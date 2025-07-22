// File: Controllers/CounterController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaStoreApi.Models;
using System.Security.Claims;
using System.Net.Http.Json;

namespace PizzaStoreApi.Controllers;

[ApiController]
[Route("api/user/counter")]
[Authorize(Roles = "User")]
public class CounterController : ControllerBase
{
    // Simulated in-memory store (use DB in production)
    private static readonly Dictionary<string, int> _userCounters = new();

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;

    public CounterController(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        _config = config;
    }

    [HttpGet]
    public IActionResult GetCounter()
    {
        var username = User.FindFirstValue(ClaimTypes.Name);
        if (!_userCounters.ContainsKey(username))
        {
            _userCounters[username] = 0;
        }

        return Ok(new { username, counter = _userCounters[username] });
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateCounter([FromQuery] string action)
    {
        var username = User.FindFirstValue(ClaimTypes.Name);
        if (!_userCounters.ContainsKey(username))
        {
            _userCounters[username] = 0;
        }

        if (action == "increment") _userCounters[username]++;
        else if (action == "decrement") _userCounters[username]--;
        else return BadRequest("Invalid action. Use 'increment' or 'decrement'.");

        // Notify webhook
        var client = _httpClientFactory.CreateClient();
        var payload = new UserChangePayload
        {
            Username = username,
            ChangedBy = "User",
            ChangeType = $"Counter {action}"
        };

        var webhookUrl = $"{Request.Scheme}://{Request.Host}/api/webhook/user-change";
        await client.PostAsJsonAsync(webhookUrl, payload);

        return Ok(new { message = "Counter updated and webhook triggered", counter = _userCounters[username] });
    }
}
