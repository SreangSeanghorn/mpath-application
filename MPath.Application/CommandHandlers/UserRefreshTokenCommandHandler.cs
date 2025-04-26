using FluentValidation;
using MPath.Application.Commands;
using MPath.Application.Exceptions.UserRefreshedToken;
using MPath.Application.ResponsesDTOs;
using MPath.Domain;
using MPath.Domain.Entities;
using MPath.Domain.Repositories;
using MPath.Infrastructure.Authentication.JwtTokenGenerator;
using MPath.SharedKernel.Primitive;

namespace MPath.Application.CommandHandlers;

public class UserRefreshTokenCommandHandler : ICommandHandler<UserRefreshTokenCommand,UserRefreshTokenResponseDto>
{
    private readonly IValidator<UserRefreshTokenCommand> _validator;
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    
    public UserRefreshTokenCommandHandler(IValidator<UserRefreshTokenCommand> validator, IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
    }
    public async Task<UserRefreshTokenResponseDto> Handle(UserRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        var user = await _userRepository.GetUserByRefreshTokenAsync(request.RefreshToken);
        if (user == null)
        {
            throw new InvalidTokenException("Invalid token.");
        }
        var newToken = _jwtTokenGenerator.GenerateToken(user.UserName,user.Roles.Select(r => r.Name).ToList());
        var newRefreshToken =  _jwtTokenGenerator.GenerateRefreshToken();
        Console.WriteLine("refresh Token:",newRefreshToken);
        user.SetRefreshToken(newRefreshToken,DateTime.UtcNow.AddDays(7));
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new UserRefreshTokenResponseDto(newToken,newRefreshToken);
    }
}