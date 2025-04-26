using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPath.Application.Commands;
using MPath.Application.ResponsesDTOs;
using MPath.Application.Shared.Responses;
using MPath.Domain.Entities;
using MPath.SharedKernel.Primitive;

namespace MPath.Application.Controllers;
[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly IQueryResolver _queryResolver;
    private readonly ICommandResolver _commandResolver;
    
    public AuthenticationController(IQueryResolver queryResolver, ICommandResolver commandResolver)
    {
        _queryResolver = queryResolver;
        _commandResolver = commandResolver;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterCommand request)
    {
        try
        {
            var result = await _commandResolver.ResolveHandler<UserRegisterCommand,UserRegisteredResponse>(request);
            return Ok(new BaseResponse<UserRegisteredResponse>(
                true,
                200,
                "User registered successfully.",
                result
            ));

        }
        catch (Exception e)
        {
            return Ok(new BaseResponse<string>(
                false,
                401,
                e.Message,
                null
            ));
        }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginCommand request)
    {
        try
        {
            var result = await _commandResolver.ResolveHandler<UserLoginCommand,UserLoginResponseDto>(request);
            return Ok(new BaseResponse<UserLoginResponseDto>(
                true,
                200,
                "Login successful.",
                result
            ));

        }
        catch (Exception e)
        {
            return Ok(new BaseResponse<string>(
                false,
                401,
                e.Message,
                null
            ));
        }
    }
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] UserRefreshTokenCommand request)
    {
        try
        {
            Console.WriteLine("refresh Token:",request.RefreshToken);
            var result = await _commandResolver.ResolveHandler<UserRefreshTokenCommand,UserRefreshTokenResponseDto>(request);
            return Ok(new BaseResponse<UserRefreshTokenResponseDto>(
                true,
                200,
                "Token refreshed successfully.",
                result
            ));

        }
        catch (Exception e)
        {
            return Ok(new BaseResponse<string>(
                false,
                401,
                e.Message,
                null
            ));
        }
    }
}