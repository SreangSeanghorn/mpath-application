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
        var result = await _commandResolver.ResolveHandler<UserRegisterCommand,BaseResponse<UserRegisteredResponse>>(request); 
        return Ok(result);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginCommand request)
    {
        var result = await _commandResolver.ResolveHandler<UserLoginCommand,BaseResponse<UserLoginResponseDto>>(request); 
        return Ok(result);
    }
    
     
    [HttpGet("get-user")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> GetUser()
    {
        return Ok("User is authenticated");
    }
     
    
}