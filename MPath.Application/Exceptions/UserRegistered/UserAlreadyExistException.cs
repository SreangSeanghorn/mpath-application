using MPath.Application.Shared.Exceptions;

namespace MPath.Application.Exceptions.UserRegistered;

public class UserAlreadyExistException : ValidationException
{
    public UserAlreadyExistException(string email)
    {
        StatusCode = 204;
        CustomMessage = $"User already exist - {email}";
    }
    

    public override int StatusCode { get; }
    public override string CustomMessage { get; }
    
    
}