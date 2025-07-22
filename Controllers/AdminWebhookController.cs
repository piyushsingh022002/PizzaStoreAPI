// File: Controllers/AdminWebhookController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaStoreApi.Models;

namespace PizzaStoreApi.Controllers;

[ApiController]
[Route("api/admin/notifications")]
[Authorize(Roles = "Admin")]
public class AdminWebhookController : ControllerBase
{
    // In-memory store to simulate admin inbox
    private static readonly List<UserChangePayload> _notifications = new();

    [HttpPost("receive")]
    [AllowAnonymous] // You can change this to secure internal-only webhook calls
    public IActionResult ReceiveWebhook([FromBody] UserChangePayload payload)
    {
        _notifications.Add(payload);

        return Ok(new { status = "Stored for admin", count = _notifications.Count });
    }

    [HttpGet]
    public IActionResult GetAllNotifications()
    {
        return Ok(_notifications.OrderByDescending(n => n.Timestamp));
    }
}
