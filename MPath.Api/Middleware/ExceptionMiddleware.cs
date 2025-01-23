

using System.Net;
using System.Reflection;
using MPath.Application.Shared.Exceptions;
using Newtonsoft.Json;

namespace MPath.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError($"An error occurred: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }

            // Optionally handle any response status code checks after the request is processed
            if (httpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                await HandleUnauthorizedResponseAsync(httpContext);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var statusCode = MapExceptionToStatusCode(exception);
            context.Response.StatusCode = statusCode;

            var response = new
            {
                StatusCode = statusCode,
                Message = exception.Message,
                Details = statusCode == (int)HttpStatusCode.InternalServerError
                    ? "There is an unexpected error occurred, please try again later."
                    : exception.Message
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }

        private async Task HandleUnauthorizedResponseAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "You are not authorized to access this resource.",
                Details = "Please check your credentials and try again."
            };

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }

        private int MapExceptionToStatusCode(Exception exception)
        {
            var statusCodeAttr = exception.GetType().GetCustomAttribute<HttpStatusCodeAttribute>();
            if (statusCodeAttr != null)
            {
                return statusCodeAttr.ErrorCode;
            }

            return exception switch
            {
              //  UserNotFoundException => (int)HttpStatusCode.NotFound,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                InvalidOperationException => (int)HttpStatusCode.Conflict,
                _ => (int)HttpStatusCode.InternalServerError
            };
        }

        private string GetCustomMessageForException(Exception exception)
        {
            return exception switch
            {
               // UserNotFoundException _ => "The specified user was not found.",
                UnauthorizedAccessException _ => "You do not have permission to access this resource.",
                ArgumentException _ => "Invalid input provided. Please check your request.",
                InvalidOperationException _ => "The operation is not valid at this time.",
                _ => "An unexpected error occurred. Please try again later."
            };
        }
    }
}
