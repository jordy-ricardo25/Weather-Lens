using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeatherLens.Entities;

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
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Date and time when the location record was created (UTC).
    /// </summary>
    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Descriptive name of the location (e.g., city, station name, or custom label).
    /// </summary>
    [Required]
    [MaxLength(150)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Geographic latitude coordinate (in decimal degrees).
    /// Positive values indicate the northern hemisphere.
    /// </summary>
    [Required]
    [Range(-90, 90)]
    [Column("latitude")]
    public float Latitude { get; set; }

    /// <summary>
    /// Geographic longitude coordinate (in decimal degrees).
    /// Positive values indicate the eastern hemisphere.
    /// </summary>
    [Required]
    [Range(-180, 180)]
    [Column("longitude")]
    public float Longitude { get; set; }
}
