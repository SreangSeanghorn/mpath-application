namespace MPath.Application.Shared.Exceptions;

public abstract class ValidationException : Exception
{
    public abstract int ErrorCode { get; }
    public abstract string CustomMessage { get; }
    
    protected ValidationException(){}
    protected ValidationException(string message) : base(message){}
}