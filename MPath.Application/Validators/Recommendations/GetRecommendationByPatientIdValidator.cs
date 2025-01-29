using FluentValidation;
using MPath.Application.Queries.Recommendations;

namespace MPath.Application.Validators.Recommendations;

public class GetRecommendationByPatientIdValidator : AbstractValidator<GetRecommendationByPatientIdQuery>
{
    public GetRecommendationByPatientIdValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty().WithMessage("PatientId is required");
        RuleFor(x => x.Page).GreaterThan(0).WithMessage("Page must be greater than 0");
        RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("PageSize must be greater than 0");
        RuleFor(x => x.OrderBy).Must(
            x => string.IsNullOrEmpty(x) || new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Title","Content" }.Contains(x)
        ).WithMessage("Invalid OrderBy field, valid values are Title, Content");
        RuleFor(x => x.SortOrder).Must(
            x => string.IsNullOrEmpty(x) || new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "ASC", "DESC" }.Contains(x)
        ).WithMessage("Invalid SortOrder field, valid values are ASC, DESC");
        RuleFor(x => x.FilterField).Must(
            x => string.IsNullOrEmpty(x) || new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Title", "Content" }.Contains(x)
        ).WithMessage("Invalid FilterField field, valid values are Title, Content");
    }
}