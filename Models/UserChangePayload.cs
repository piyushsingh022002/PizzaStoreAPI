// File: Models/UserChangePayload.cs
namespace PizzaStoreApi.Models;

public class UserChangePayload
{
    public string? Username { get; set; }
    public string? ChangedBy { get; set; }  // "User" or "Admin"
    public string? ChangeType { get; set; } // e.g., "UpdateProfile", "PasswordChange"
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
