using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeatherLens.Models;

/// <summary>
/// Represents a registered user within the App.
/// </summary>
[Table("users")]
public sealed class User
{
    /// <summary>
    /// Unique identifier for the user (primary key).
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// The date and time when the user account was created (UTC).
    /// </summary>
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The user's full name.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The user's email address. Must be unique and valid for authentication.
    /// </summary>
    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The user's assigned role (e.g., "Admin", "Researcher", "Viewer").
    /// Optional; defaults to "User" if not provided.
    /// </summary>
    [MaxLength(50)]
    public string? Role { get; set; } = "User";
}
