using FluentValidation;
using MPath.Application.Commands;
using MPath.Application.Exceptions.Recommendations;
using MPath.Application.Exceptions.Users;
using MPath.Domain;
using MPath.Domain.Repositories;
using MPath.SharedKernel.Primitive;

namespace MPath.Application.CommandHandlers;

public class MarkRecommendationAsCompletedCommandHandler : ICommandHandler<MarkRecommendationAsCompletedCommand,bool>
{
    private IPatientRepository _patientRepository;
    private readonly IRecommendationRepository _recommendationRepository;
    private IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<MarkRecommendationAsCompletedCommand> _validator;
    
    public MarkRecommendationAsCompletedCommandHandler(IPatientRepository patientRepository, IRecommendationRepository recommendationRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, IValidator<MarkRecommendationAsCompletedCommand> validator)
    {
        _patientRepository = patientRepository;
        _recommendationRepository = recommendationRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    public async Task<bool> Handle(MarkRecommendationAsCompletedCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new InvalidInputForMarkRecommendationAsCompletedException(validationResult.Errors.First().ErrorMessage);
        }
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new UserNotFoundException("User not found");
        }
        var recommendation = await _recommendationRepository.GetByIdAsync(request.RecommendationId);
        user.MarkRecommendationAsCompleted(recommendation);
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}