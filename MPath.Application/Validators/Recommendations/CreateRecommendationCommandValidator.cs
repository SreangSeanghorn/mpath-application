using FluentValidation;
using MPath.Application.Commands;

namespace MPath.Application.Validators.Recommendations;

public class CreateRecommendationCommandValidator : AbstractValidator<CreateRecommendationCommand>
{
    public CreateRecommendationCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
        RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required");
        RuleFor(x => x.PatientId).NotEmpty().WithMessage("PatientId is required");
    }
    
}