

using FluentValidation;
using MPath.Application.Commands;
using MPath.Application.Exceptions.Patients;
using MPath.Application.Exceptions.Recommendations;
using MPath.Application.Exceptions.Users;
using MPath.Application.ResponsesDTOs.Recommendations;
using MPath.Domain;
using MPath.Domain.Repositories;
using MPath.SharedKernel.Primitive;

namespace MPath.Application.CommandHandlers;

public class CreateRecommendationCommandHandler : ICommandHandler<CreateRecommendationCommand,CreateRecommendationCommandResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IValidator<CreateRecommendationCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateRecommendationCommandHandler(IUserRepository userRepository, IPatientRepository patientRepository, IValidator<CreateRecommendationCommand> validator, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _patientRepository = patientRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }
    public async Task<CreateRecommendationCommandResponseDto> Handle(CreateRecommendationCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            throw new InvalidInputForCreateRecommendationException(validationResult.Errors.First().ErrorMessage);
        }
        var user = await _userRepository.GetUserWithPatient(request.UserId);
        if (user == null)
        {
            throw new UserNotFoundException("User not found");
        }
        var patient = await _patientRepository.GetByIdAsync(request.PatientId);
        if (patient == null)
        {
            throw new PatientWithProvidedIdNotFoundException("Patient not found");
        }
        user.CreateRecommendation(request.Title, request.Content,false, request.PatientId);
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var response = new CreateRecommendationCommandResponseDto(request.Title, request.Content, request.PatientId);
        return response;
    }
}