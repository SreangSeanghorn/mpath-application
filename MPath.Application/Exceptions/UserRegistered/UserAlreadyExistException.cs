using MPath.Application.Shared.Exceptions;

namespace MPath.Application.Exceptions.UserRegistered;

public class UserAlreadyExistException : ValidationException
{
    public UserAlreadyExistException(string email)
    {
        ErrorCode = 204;
        CustomMessage = $"User already exist - {email}";
    }
    

    public override int ErrorCode { get; }
    public override string CustomMessage { get; }
    
    
}