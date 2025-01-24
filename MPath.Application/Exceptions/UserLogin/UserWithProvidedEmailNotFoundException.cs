using MPath.Application.Shared.Exceptions;

namespace MPath.Application.Exceptions.UserLogin;

public class UserWithProvidedEmailNotFoundException : ValidationException
{
    public override int StatusCode { get; }
    public override string CustomMessage { get; }
    
    public UserWithProvidedEmailNotFoundException(string email) 
    {
        StatusCode = 404;
        CustomMessage = $"User with provided email: {email} not found";
    }
}