using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeatherLens.Models;

/// <summary>
/// Represents a geographic location monitored or referenced within the WeatherLens system.
/// </summary>
[Table("locations")]
public sealed class Location
{
    /// <summary>
    /// Unique identifier for the location (primary key).
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Date and time when the location record was created (UTC).
    /// </summary>
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Descriptive name of the location (e.g., city, station name, or custom label).
    /// </summary>
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Geographic latitude coordinate (in decimal degrees).
    /// Positive values indicate the northern hemisphere.
    /// </summary>
    [Required]
    [Range(-90, 90)]
    public float Latitude { get; set; }

    /// <summary>
    /// Geographic longitude coordinate (in decimal degrees).
    /// Positive values indicate the eastern hemisphere.
    /// </summary>
    [Required]
    [Range(-180, 180)]
    public float Longitude { get; set; }

    /// <summary>
    /// Country name or ISO country code associated with the location (optional).
    /// </summary>
    [MaxLength(100)]
    public string? Country { get; set; }

    /// <summary>
    /// Region, state, or province name associated with the location (optional).
    /// </summary>
    [MaxLength(100)]
    public string? Region { get; set; }
}
