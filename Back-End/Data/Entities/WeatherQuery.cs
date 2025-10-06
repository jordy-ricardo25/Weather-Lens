using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeatherLens.Entities;

/// <summary>
/// Represents a single weather data request performed by a user for a specific location and date.
/// </summary>
[Table("weather_queries")]
public sealed class WeatherQuery
{
    /// <summary>
    /// Unique identifier for the weather query (primary key).
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Timestamp indicating when the query was created (UTC).
    /// </summary>
    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The date or time period requested for weather data retrieval.
    /// </summary>
    [Required]
    [Column("date")]
    public DateTime Date { get; set; }

    /// <summary>
    /// Foreign key referencing the <see cref="Location"/> for which the weather data was requested.
    /// </summary>
    [Required]
    [Column("location_id")]
    public Guid LocationId { get; set; }

    /// <summary>
    /// Navigation property for the location associated with this query.
    /// </summary>
    [ForeignKey(nameof(LocationId))]
    public Location Location { get; set; } = null!;
}
