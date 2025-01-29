
using ValidationException = MPath.Application.Shared.Exceptions.ValidationException;

namespace MPath.Application.Exceptions.Recommendations;

public class InvalidInputForCreateRecommendationException : ValidationException
{
    public override int StatusCode { get; }
    public override string CustomMessage { get; }
    
    public InvalidInputForCreateRecommendationException(string message) : base(message)
    {
        StatusCode = 204;
        CustomMessage = message;
    }
}