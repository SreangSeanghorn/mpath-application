using MPath.Application.Shared.Exceptions;

namespace MPath.Application.Exceptions.Users;

public class UserNotFoundException : ValidationException
{
    public override int StatusCode { get; }
    public override string CustomMessage { get; }
    
    public UserNotFoundException(string message) : base(message)
    {
        StatusCode = 404;
        CustomMessage = message;
    }
}