

namespace MPath.Application.Shared.Exceptions
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class HttpStatusCodeAttribute : Attribute
    {
        public int ErrorCode { get; }

        public HttpStatusCodeAttribute(int errorCode)
        {
            ErrorCode = errorCode;
        }
     
    }
}