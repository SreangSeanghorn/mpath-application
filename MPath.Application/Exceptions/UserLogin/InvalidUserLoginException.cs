

using MPath.Application.Shared.Exceptions;

namespace MPath.Application.Exceptions.UserLogin;

public class InvalidUserLoginException : ValidationException
{
    public override int StatusCode { get; }
    public override string CustomMessage { get; }
    
    public InvalidUserLoginException(string customMessage) : base(customMessage)
    {
        StatusCode = 400;
        CustomMessage = customMessage;
    }
}