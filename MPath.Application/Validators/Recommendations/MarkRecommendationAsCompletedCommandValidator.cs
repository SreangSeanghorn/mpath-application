using FluentValidation;
using MPath.Application.Commands;

namespace MPath.Application.Validators.Recommendations;

public class MarkRecommendationAsCompletedCommandValidator : AbstractValidator<MarkRecommendationAsCompletedCommand>
{
    public MarkRecommendationAsCompletedCommandValidator()
    {
        RuleFor(x => x.RecommendationId).NotEmpty().WithMessage("RecommendationId is required");
    }
}