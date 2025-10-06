namespace WeatherLens.DTOs;

/// <summary>
/// Represents the user data returned by the API.
/// </summary>
public sealed class UserResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Role { get; set; }
    public DateTime CreatedAt { get; set; }
}
