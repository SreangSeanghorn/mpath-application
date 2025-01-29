using FluentValidation;
using MPath.Application.Queries.Patients;

namespace MPath.Application.Validators.Patients;

public class GetPatientDetailByIdQueryValidator : AbstractValidator<GetPatientDetailByIdQuery>
{
    public GetPatientDetailByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
    
}