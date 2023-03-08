using DriverService.API.Domain.DTO;
using DriverService.API.Domain.Utils;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json;

namespace DriverService.API.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly NotificationContext _notificationContext;
        public BaseController(NotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
        }
        protected IActionResult CustomResponse(object? result = null)
        {
            if (_notificationContext.HasNotifications)
            {
                Log.Error($"[{Request.Path}] Error: {JsonSerializer.Serialize(_notificationContext.Notifications)}");
                return StatusCode(StatusCodes.Status400BadRequest, new ResultViewModel()
                {
                    Errors = _notificationContext.Notifications.Select(n => n.Message),
                    Success = false,
                    DisplayMessage = "Bad Request Error"
                });
            }

            if (result == null)
                return NoContent();

            return Ok(new ResultViewModel<object>() { Data = result, Success = true });
        }


    }
}
