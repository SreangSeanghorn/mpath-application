using FluentValidation;
using MPath.Application.Commands;

namespace MPath.Application.Validators.UserRegistered;

public class UserRegisteredCommandValidator : AbstractValidator<UserRegisterCommand>
{
    public UserRegisteredCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.Password).NotEmpty().MinimumLength(4).WithMessage("Password is required and must be at least 4 characters");
    }
}
