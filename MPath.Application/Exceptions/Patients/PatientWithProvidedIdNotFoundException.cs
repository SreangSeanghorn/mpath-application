using MPath.Application.Shared.Exceptions;

namespace MPath.Application.Exceptions.Patients;

public class PatientWithProvidedIdNotFoundException : ValidationException
{
    public override int StatusCode { get; }
    public override string CustomMessage { get; }
    
    public PatientWithProvidedIdNotFoundException(string message) : base(message)
    {
        StatusCode = 404;
        CustomMessage = message;
    }
}