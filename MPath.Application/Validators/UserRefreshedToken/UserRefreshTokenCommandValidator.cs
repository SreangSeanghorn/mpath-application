using FluentValidation;
using MPath.Application.Commands;

namespace MPath.Application.Validators.UserRefreshedToken;

public class UserRefreshTokenCommandValidator : AbstractValidator<UserRefreshTokenCommand>
{
    public UserRefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required.");
    }
    
}