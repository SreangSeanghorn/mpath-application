using FluentValidation;
using MPath.Application.Exceptions.Patients;
using MPath.Application.Queries.Recommendations;
using MPath.Application.ResponsesDTOs.Recommendations;
using MPath.Application.Shared.Responses;
using MPath.Domain.Repositories;
using MPath.SharedKernel.Primitive;

namespace MPath.Application.QueriesHandlers.Recommendations;

public class GetRecommendationByPatientIdQueryHandler : IQueryHandler<GetRecommendationByPatientIdQuery,PaginationDto<IEnumerable<GetRecommendationByPatientIdResponseDto>>>
{
    private readonly IPatientRepository _patientRepository;
    private readonly IValidator<GetRecommendationByPatientIdQuery> _validator;
    
    public GetRecommendationByPatientIdQueryHandler(IPatientRepository patientRepository, IValidator<GetRecommendationByPatientIdQuery> validator)
    {
        _patientRepository = patientRepository;
        _validator = validator;
    }
    public async Task<PaginationDto<IEnumerable<GetRecommendationByPatientIdResponseDto>>> Handle(GetRecommendationByPatientIdQuery query)
    {
        var validationResult = _validator.Validate(query);
        if (!validationResult.IsValid)
        {
            throw new InvalidInputForGetRecommendationByPatientIdException(validationResult.Errors.First().ErrorMessage);
        }
        var patient = await _patientRepository.GetByIdAsync(query.PatientId);
        if (patient == null)
        {
            throw new PatientWithProvidedIdNotFoundException("Patient with provided id not found");
        }
        var recommendations = await _patientRepository.GetRecommendationsByPatientId(query.PatientId, query.Page, query.PageSize, query.OrderBy, query.SortOrder, query.FilterField, query.FilterValue);
        var response = recommendations.Recommendations.Select(x => new GetRecommendationByPatientIdResponseDto
        (
            x.Id,
            x.Title,
            x.Content,
            x.IsCompleted
        ));
        
        return new PaginationDto<IEnumerable<GetRecommendationByPatientIdResponseDto>>
        {
            Data = response,
            TotalPage = recommendations.TotalPages,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }
}