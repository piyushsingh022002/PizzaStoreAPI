// File: Controllers/WebhookController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzaStoreApi.Models;

namespace PizzaStoreApi.Controllers;

[ApiController]
[Route("api/webhook")]
public class WebhookController : ControllerBase
{
    private readonly ILogger<WebhookController> _logger;

    public WebhookController(ILogger<WebhookController> logger)
    {
        _logger = logger;
    }

    [HttpPost("user-change")]
    [Authorize(Roles = "User")]
    public IActionResult ReceiveUserChange([FromBody] UserChangePayload payload)
    {
        _logger.LogInformation("🔔 Webhook received: {Username} triggered {ChangeType} by {ChangedBy} at {Timestamp}",
            payload.Username, payload.ChangeType, payload.ChangedBy, payload.Timestamp);

        // You can later forward this to another service, admin dashboard, etc.
        return Ok(new
        {
            status = "Received",
            receivedAt = DateTime.UtcNow
        });
    }
}