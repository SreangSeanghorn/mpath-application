using MPath.Application.Shared.Exceptions;

namespace MPath.Application.Exceptions.UserRefreshedToken;

public class InvalidTokenException : ValidationException
{
    public override int StatusCode { get; }
    public override string CustomMessage { get; }
    
    public InvalidTokenException(string message) : base(message)
    {
        StatusCode = 204;
    }
}