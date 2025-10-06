using System.ComponentModel.DataAnnotations;

namespace WeatherLens.DTOs;

/// <summary>
/// Represents the incoming data required to create or update a user.
/// </summary>
public sealed class UserRequest
{
    /// <summary>
    /// Full name of the user.
    /// </summary>
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// User’s email address used for identification and notifications.
    /// </summary>
    [Required, EmailAddress, MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Optional role assigned to the user (e.g., "Admin", "User").
    /// </summary>
    [MaxLength(50)]
    public string? Role { get; set; } = "User";
}
