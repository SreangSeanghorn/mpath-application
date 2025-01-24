using System.Security.Authentication;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using MPath.Application.Commands;
using MPath.Application.Exceptions.UserLogin;
using MPath.Application.ResponsesDTOs;
using MPath.Application.Shared.Responses;
using MPath.Domain.Core.Interfaces;
using MPath.Domain.Entities;
using MPath.Domain.Repositories;
using MPath.Infrastructure.Authentication.JwtRefreshTokenGenerator;
using MPath.Infrastructure.Authentication.JwtTokenGenerator;
using MPath.SharedKernel.Primitive;

namespace MPath.Application.CommandHandlers;

public class UserLoginCommandHandler : ICommandHandler<UserLoginCommand,BaseResponse<UserLoginResponseDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IValidator<UserLoginCommand> _validator;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IJwtRefreshTokenGenerator _jwtRefreshTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    
    public UserLoginCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, IValidator<UserLoginCommand> validator, IJwtTokenGenerator jwtTokenGenerator, IJwtRefreshTokenGenerator jwtRefreshTokenGenerator, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _validator = validator;
        _jwtTokenGenerator = jwtTokenGenerator;
        _jwtRefreshTokenGenerator = jwtRefreshTokenGenerator;
        _passwordHasher = passwordHasher;
    }
    public async Task<BaseResponse<UserLoginResponseDto>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new InvalidUserLoginException(validationResult.Errors.First().ErrorMessage);
        }
        var user = await _userRepository.GetByEmail(request.Email) ?? throw new UserWithProvidedEmailNotFoundException(request.Email);
        if (!_passwordHasher.VerifyPassword(user.Password, request.Password))
        {
            throw new IncorrectUserCredentialException("Incorrect credentials");
        }
        var roles = user.Roles.Select(r => r.Name).ToList();
        var token = _jwtTokenGenerator.GenerateToken(user.Email.Value, roles);
        var refreshToken = _jwtRefreshTokenGenerator.GenerateRefreshToken();
        var refreshTokenObj = RefreshToken.Create(refreshToken, _jwtRefreshTokenGenerator.GetExpiryDate());
        user.AddRefreshToken(refreshTokenObj,DateTime.UtcNow.AddMinutes(30));
        await _userRepository.SaveChangesAsync();
        var userLoginResponse = new UserLoginResponseDto(token, refreshToken, DateTime.UtcNow.AddMinutes(30));
        return new BaseResponse<UserLoginResponseDto>(true, 200, "", userLoginResponse);
    }
}