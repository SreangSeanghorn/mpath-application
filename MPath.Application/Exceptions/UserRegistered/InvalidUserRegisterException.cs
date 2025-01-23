using MPath.Application.Shared.Exceptions;

namespace MPath.Application.Exceptions.UserRegistered;

public class InvalidUserRegisterException : ValidationException
{
    public InvalidUserRegisterException(int errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
        CustomMessage = message;
    }

    public override int ErrorCode { get; }
    public override string CustomMessage { get; }
    
    
}