using MessageQueueConnectionLib.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class TestRabbitController : ControllerBase
{
    private readonly IMessagePublisher _publisher;

    public TestRabbitController(IMessagePublisher publisher)
    {
        _publisher = publisher;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] string messageText)
    {
        await _publisher.PublishAsync("email_queue", new { Text = messageText, Date = DateTime.Now });

        return Ok("Сообщение отправлено в RabbitMQ!");
    }
}