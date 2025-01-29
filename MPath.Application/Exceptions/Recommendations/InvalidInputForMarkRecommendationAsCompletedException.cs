using MPath.Application.Shared.Exceptions;

namespace MPath.Application.Exceptions.Recommendations;

public class InvalidInputForMarkRecommendationAsCompletedException : ValidationException
{
    public override int StatusCode { get; }
    public override string CustomMessage { get; }
    
    public InvalidInputForMarkRecommendationAsCompletedException(string message) : base(message)
    {
        StatusCode = 204;
        CustomMessage = message;
    }
}