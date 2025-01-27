using MPath.Application.Queries.Patients;
using MPath.Application.ResponsesDTOs.Patients;
using MPath.Application.Shared.Responses;
using MPath.Domain.Repositories;
using MPath.SharedKernel.Primitive;
using Dapper;
using FluentValidation;
using Microsoft.Data.SqlClient;
using MPath.Application.Exceptions.Patients;

namespace MPath.Application.QueriesHandlers.Patients;

public class GetListOfPatientsQueryHandler : IQueryHandler<GetListOfPatientsQuery,PaginationDto<IEnumerable<GetListOfPatientResponseDto>>>
{
    private readonly IPatientRepository _patientRepository;
    private readonly IValidator<GetListOfPatientsQuery> _validator;
    public GetListOfPatientsQueryHandler(IPatientRepository patientRepository, IValidator<GetListOfPatientsQuery> validator)
    {
        _patientRepository = patientRepository;
        _validator = validator;
    }
    public async Task<PaginationDto<IEnumerable<GetListOfPatientResponseDto>>> Handle(GetListOfPatientsQuery query)
    {
        var validationResult = await _validator.ValidateAsync(query);
        if (!validationResult.IsValid)
        {
            throw new InvalidInputForGetListOfPatientExeption(validationResult.Errors.First().ErrorMessage);
        }
        var (patients, totalPages) = await _patientRepository.GetPatientsWithPaginationAsync(query.Page, query.PageSize, query.GetOrderBy(), query.GetSortOrder(), query.FilterField, query.FilterValue);
        var response = patients.Select(p => new GetListOfPatientResponseDto(
            p.Id,
            p.Name,
            p.Email.Value,
            p.PhoneNumber,
            p.Address,
            p.BirthDate
        ));
       
        return new PaginationDto<IEnumerable<GetListOfPatientResponseDto>>(response, totalPages, query.Page, query.PageSize);
    }
    }
    
   
    
    
    

    
