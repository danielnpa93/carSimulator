using DriverService.API.Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Serilog;
using System.Net;
using System.Text.Json;

namespace DriverService.API.Filters
{
    public class ValidateModelFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = GetModeStateErrors(context.ModelState);

                Log.Error($"[{context.HttpContext.Request.Path}] Error: {JsonSerializer.Serialize(errors)}");
                context.Result =
                new ObjectResult(new ResultViewModel()
                {
                    DisplayMessage = "Unknow error occurred",
                    Success = false,
                    Errors = GetModeStateErrors(context.ModelState)
                })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }
        private IEnumerable<string> GetModeStateErrors(ModelStateDictionary modelState)
        {
            return modelState.Values
            .SelectMany(ms => ms.Errors)
            .Select(e => e.Exception == null ? e.ErrorMessage : e.Exception.Message);
        }
    }
}
