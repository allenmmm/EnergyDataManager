using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace EnergyDataManager.Web.DomainServices
{
    public class HttpErrorResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }
        public string Path { get; set; }

        public HttpErrorResponse()
        {
        }

        public HttpErrorResponse(
            HttpStatusCode statusCode,
            string error,
            string message,
            string path)
        {
            StatusCode = statusCode;
            Error = error;
            Message = message;
            Path = path;
        }

        public HttpErrorResponse(
            ExceptionContext exContext)
        {
            Path = exContext.HttpContext.Request.Path;
            if (exContext.Exception is DbUpdateConcurrencyException)
            {
                StatusCode = HttpStatusCode.Conflict;
                Error = "Error concurrent database access detected";
                Message = "Operation not performed, please repeat";
            }
            else if (exContext.Exception is DbUpdateException)
            {
                StatusCode = HttpStatusCode.InternalServerError;
                Error = "Internal Server Error";
                Message = "Unexpected database error, operation not performed";
            }
            else if (exContext.Exception is ArgumentException)
            {   // custom application errors such as 404 not found 
                var exception = (ArgumentException)exContext.Exception;
                StatusCode = HttpStatusCode.BadRequest;
                Error = "Application Error";
                Message = exception.Message;
            }
            else if (exContext.Exception is FormatException || 
                exContext.Exception is IOException)
            {          
                StatusCode = HttpStatusCode.InternalServerError;
                Error = "Internal Server Error";
                Message = exContext.Exception.Message;
            }
            else
            {   // other types of errors such as Domain Guard clause exceptions
                StatusCode = HttpStatusCode.InternalServerError;
                Error = "Internal Server Error.";
                Message = "Unhandled exception occurred";
            }
        }

        public string SerialiseResponse()
        {
            // if you want carriage returns
            // var opt = new JsonSerializerOptions() { WriteIndented = true  };
            // return JsonSerializer.Serialize(this, opt);
            return JsonSerializer.Serialize(this);
        }
    }
}
