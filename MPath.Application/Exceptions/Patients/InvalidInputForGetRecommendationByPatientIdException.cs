using MPath.Application.Shared.Exceptions;

namespace MPath.Application.Exceptions.Patients;

public class InvalidInputForGetRecommendationByPatientIdException : ValidationException
{
    public override int StatusCode { get; }
    public override string CustomMessage { get; }
    
    public InvalidInputForGetRecommendationByPatientIdException(string message) : base(message)
    {
        StatusCode = 204;
        CustomMessage = message;
    }
}