


using MPath.Application.Shared.Exceptions;

namespace MPath.Application.Exceptions.UserRefreshedToken;

public class InvalidUserRefreshedTokenException : ValidationException
{
    public override int StatusCode { get; }
    public override string CustomMessage { get; }
    
    public InvalidUserRefreshedTokenException(string message) : base(message)
    {
        StatusCode = 204;
    }
}