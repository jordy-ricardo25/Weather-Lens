using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeatherLens.Models;

/// <summary>
/// Represents a weather or environmental variable tracked by the WeatherLens system.
/// </summary>
[Table("weather_variables")]
public sealed class WeatherVariable
{
    /// <summary>
    /// Unique identifier for the weather variable (primary key).
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Timestamp indicating when the variable definition was created (UTC).
    /// </summary>
    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Short name or code of the variable (e.g., "Rainf", "Tair", "Wind_E").
    /// </summary>
    [Required]
    [MaxLength(50)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Measurement unit used for this variable (e.g., "mm/h", "K", "m/s").
    /// </summary>
    [Required]
    [MaxLength(30)]
    [Column("unit")]
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// Descriptive explanation of the variable’s meaning or usage.
    /// </summary>
    [Required]
    [MaxLength(255)]
    [Column("description")]
    public string Description { get; set; } = string.Empty;
}
