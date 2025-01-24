using MPath.Application.Shared.Exceptions;

namespace MPath.Application.Exceptions.Roles;

[HttpStatusCode(404)]
public class RoleNotFoundException : ValidationException
{
    public override int StatusCode { get; }
    public override string CustomMessage { get; }
    
    public RoleNotFoundException()
    {
        StatusCode = 404;
        CustomMessage = "Role not found";
    }
}