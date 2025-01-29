using FluentValidation;
using MPath.Application.Exceptions.Patients;
using MPath.Application.Queries.Patients;
using MPath.Application.ResponsesDTOs.Patients;
using MPath.Application.ResponsesDTOs.Recommendations;
using MPath.Domain.Repositories;
using MPath.SharedKernel.Primitive;

namespace MPath.Application.QueriesHandlers.Patients;

public class GetPatientDetailByIdQueryHandler : IQueryHandler<GetPatientDetailByIdQuery,PatientDetailResponseDto>
{
    private readonly IPatientRepository _patientRepository;
    private readonly IValidator<GetPatientDetailByIdQuery> _validator;
    public GetPatientDetailByIdQueryHandler(IPatientRepository patientRepository, IValidator<GetPatientDetailByIdQuery> validator)
    {
        _patientRepository = patientRepository;
        _validator = validator;
    }
    public async Task<PatientDetailResponseDto> Handle(GetPatientDetailByIdQuery query)
    {
        var validationResult = await _validator.ValidateAsync(query);
        if (!validationResult.IsValid)
        {
            throw new InvalidInputForGetPatientDetailByIdException(validationResult.Errors.First().ErrorMessage);
        }
        var patient = await _patientRepository.GetPatientWithRecommendations(query.Id);
        if (patient == null)
        {
            throw new PatientWithProvidedIdNotFoundException("Patient with provided id not found");
        }
        var response = new PatientDetailResponseDto(
            patient.Id,
            patient.Name,
            patient.Email.Value,
            patient.PhoneNumber,
            patient.Address,
            patient.BirthDate,
            patient.Recommendations.Select(r => new GetRecommendationByPatientIdResponseDto(
                r.Id,
                r.Title,
                r.Content,
                r.IsCompleted
            ))
        );
        return response;
    }
}