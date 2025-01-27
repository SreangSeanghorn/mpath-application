using FluentValidation;
using MPath.Application.Commands;
using MPath.Application.Exceptions.Patients;
using MPath.Application.Exceptions.Users;
using MPath.Application.ResponsesDTOs.Patients;
using MPath.Application.Shared.Responses;
using MPath.Domain;
using MPath.Domain.Entities;
using MPath.Domain.Repositories;
using MPath.Domain.ValueObjects;
using MPath.SharedKernel.Primitive;

namespace MPath.Application.CommandHandlers;

public class CreatePatientCommandHandler : ICommandHandler<CreatePatientCommand,PatientCreatedResponseDto>
{
    private readonly IPatientRepository _patientRepository;
    private readonly IValidator<CreatePatientCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private IUserRepository _userRepository;
    
    public CreatePatientCommandHandler(IPatientRepository patientRepository, IValidator<CreatePatientCommand> validator, IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _patientRepository = patientRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }
    public async Task<PatientCreatedResponseDto> Handle(CreatePatientCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            throw new InvalidPatientCreatedException(validationResult.Errors.First().ErrorMessage);
        }
        var existingPatient = await _patientRepository.GetPatientByEmail(request.Email);
        if (existingPatient != null)
        {
            throw new PatientWithProvidedEmailAlreadyExistException(request.Email);
        }
        var user = _userRepository.GetByIdAsync(request.UserId).Result;
        if (user == null)
        {
            throw new UserNotFoundException("The user with the provided information does not exist");
        }
        var email = Email.Create(request.Email);
        var patient = user.CreatePatient(request.Name, email, request.Phone, request.Address, request.DateOfBirth);
        await _patientRepository.AddAsync(patient);
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var patientCreatedResponse = new PatientCreatedResponseDto(patient.Name, patient.Email.Value,
            patient.PhoneNumber, patient.Address, patient.BirthDate);
        return patientCreatedResponse;
    }
}