using MediatR;
using Microsoft.AspNetCore.Mvc;
using MPath.Application.Commands;

namespace MPath.Application.Controllers;
[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }
}