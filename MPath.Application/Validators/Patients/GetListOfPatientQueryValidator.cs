using FluentValidation;
using MPath.Application.Queries.Patients;

namespace MPath.Application.Validators.Patients;

public class GetListOfPatientQueryValidator : AbstractValidator<GetListOfPatientsQuery>
{
    public GetListOfPatientQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0).WithMessage("Page must be greater than 0");
        RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("PageSize must be greater than 0");
        RuleFor(x => x.OrderBy).Must(
            x => string.IsNullOrEmpty(x) || new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Name", "Email", "PhoneNumber", "Address", "DateOfBirth" }.Contains(x)
        ).WithMessage("Invalid OrderBy field, valid values are Name, Email, PhoneNumber, Address, DateOfBirth");
        RuleFor(x => x.SortOrder).Must(
            x => string.IsNullOrEmpty(x) || new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "ASC", "DESC" }.Contains(x)
        ).WithMessage("Invalid SortOrder field, valid values are ASC, DESC");
        RuleFor(x => x.FilterField).Must(
            x => string.IsNullOrEmpty(x) || new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Name", "Email", "PhoneNumber", "Address", "DateOfBirth" }.Contains(x)
        ).WithMessage("Invalid FilterField field, valid values are Name, Email, PhoneNumber, Address, DateOfBirth");
    }
    
}