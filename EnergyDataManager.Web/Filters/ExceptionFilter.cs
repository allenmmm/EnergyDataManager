using EnergyDataManager.Web.DomainServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EnergyDataManager.Web.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public async override void OnException(ExceptionContext context)
        {
            var httpErrorResponse = new HttpErrorResponse(
                context);

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode =
                (int)httpErrorResponse.StatusCode;
            await context.HttpContext.Response
                .WriteAsync(httpErrorResponse.SerialiseResponse());
            context.ExceptionHandled = true;
        }
    }
}
