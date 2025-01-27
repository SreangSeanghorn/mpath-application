
using MPath.Application.Shared.Exceptions;

namespace MPath.Application.Exceptions.Patients;

public class PatientWithProvidedEmailAlreadyExistException : ValidationException
{
    public override int StatusCode { get; }
    public override string CustomMessage { get; }
    
    public PatientWithProvidedEmailAlreadyExistException(string email)
    {
        StatusCode = 400;
        CustomMessage = $"Patient with email {email} already exist";
    }
}