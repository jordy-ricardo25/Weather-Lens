using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeatherLens.Models;

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
    public Guid Id { get; set; }

    /// <summary>
    /// Timestamp indicating when the query was created (UTC).
    /// </summary>
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The date or time period requested for weather data retrieval.
    /// </summary>
    [Required]
    public DateTime Date { get; set; }

    /// <summary>
    /// Current processing status of the query (e.g., "Pending", "Completed", "Failed").
    /// </summary>
    [MaxLength(50)]
    public string? Status { get; set; } = "Pending";

    /// <summary>
    /// Foreign key referencing the <see cref="User"/> who initiated the query.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Foreign key referencing the <see cref="Location"/> for which the weather data was requested.
    /// </summary>
    [Required]
    public Guid LocationId { get; set; }

    /// <summary>
    /// Navigation property for the user associated with this query.
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    /// <summary>
    /// Navigation property for the location associated with this query.
    /// </summary>
    [ForeignKey(nameof(LocationId))]
    public Location Location { get; set; } = null!;
}
