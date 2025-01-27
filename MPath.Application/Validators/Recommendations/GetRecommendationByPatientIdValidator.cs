using FluentValidation;
using MPath.Application.Queries.Recommendations;

namespace MPath.Application.Validators.Recommendations;

public class GetRecommendationByPatientIdValidator : AbstractValidator<GetRecommendationByPatientIdQuery>
{
    public GetRecommendationByPatientIdValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty().WithMessage("PatientId is required");
    }
}