using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeatherLens.Models;

/// <summary>
/// Represents a weather or environmental variable tracked by the WeatherLens system.
/// </summary>
[Table("WeatherVariables")]
public sealed class WeatherVariable
{
    /// <summary>
    /// Unique identifier for the weather variable (primary key).
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Timestamp indicating when the variable definition was created (UTC).
    /// </summary>
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Short name or code of the variable (e.g., "Rainf", "Tair", "Wind_E").
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Measurement unit used for this variable (e.g., "mm/h", "K", "m/s").
    /// </summary>
    [Required]
    [MaxLength(30)]
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// Descriptive explanation of the variable’s meaning or usage.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// URL reference to the data source or NASA dataset from which this variable originates.
    /// </summary>
    [Required]
    [Url]
    [MaxLength(500)]
    public string DataUrl { get; set; } = string.Empty;
}
