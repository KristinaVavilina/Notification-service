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
    [ProducesResponseType(typeof(NotificationResponseDto), 200)]
    public async Task<IActionResult> PostNotification([FromBody] NotificationDto dto)
    {
        var id = await _service.PublishMessageAsync(dto);
        return Ok(new NotificationResponseDto
        {
            Id = id
        });
    }
}
