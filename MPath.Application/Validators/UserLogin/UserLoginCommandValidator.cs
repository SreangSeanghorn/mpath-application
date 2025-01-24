using FluentValidation;
using MPath.Application.Commands;

namespace MPath.Application.Validators.UserLogin;

public class UserLoginCommandValidator : AbstractValidator<UserLoginCommand>
{
    public UserLoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.Email).EmailAddress().WithMessage("Email is not valid");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
    }
    
}