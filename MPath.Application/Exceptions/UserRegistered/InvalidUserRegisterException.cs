using MPath.Application.Shared.Exceptions;

namespace MPath.Application.Exceptions.UserRegistered;

public class InvalidUserRegisterException : ValidationException
{
    public InvalidUserRegisterException(string message) : base(message)
    {
        StatusCode = 422;
        CustomMessage = message;
    }

    public override int StatusCode { get; }
    public override string CustomMessage { get; }
    
    
}