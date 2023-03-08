using DriverService.API.Domain.DTO;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Serilog;

namespace DriverService.API.Filters
{
    public class DefaultExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            Log.Error($"[{context.HttpContext.Request.Path}] {context.Exception.InnerException?.Message ?? context.Exception.Message}");
            context.Result =
            new ObjectResult(new ResultViewModel
            {
                DisplayMessage = "Unknow Error Occurred",
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }
}
