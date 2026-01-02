using GatewayServiceApi.Interfaces;
using GatewayServiceApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace GatewayServiceApi.Controllers;

[ApiController]
[Route("notification")]
public class NotificationController : ControllerBase
{
    private INotificationService _service;

    public NotificationController(INotificationService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("publish")]
    public async Task<IActionResult> PostNotification([FromBody] NotificationDto dto)
    {
        await _service.PublishMessageAsync(dto);
        return Ok();
    }
}
