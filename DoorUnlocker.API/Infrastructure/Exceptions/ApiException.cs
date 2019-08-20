using System;
using System.Net;

namespace DoorUnlocker.API.Infrastructure.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException(string message, HttpStatusCode statusCode)
            : this(message, null, statusCode)
        {
        }

        public ApiException(string message, Exception innerException, HttpStatusCode statusCode)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        public virtual HttpStatusCode StatusCode { get; }
        
        public static ApiException NotFound(string message) => new ApiException(message, HttpStatusCode.NotFound);
        
        public static ApiException Forbidden(string message) => new ApiException(message, HttpStatusCode.Forbidden);

        public static ApiException BadRequest(string message) => new ApiException(message, HttpStatusCode.BadRequest);
    }
}