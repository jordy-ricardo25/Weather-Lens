using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeatherLens.Models;

/// <summary>
/// Represents the computed weather data results for a specific variable within a weather query.
/// </summary>
[Table("weather_results")]
public sealed class WeatherResult
{
    /// <summary>
    /// Unique identifier for the weather result record (primary key).
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Timestamp indicating when this result record was created (UTC).
    /// </summary>
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The mean (average) value of the variable over the evaluated time range.
    /// </summary>
    public float? MeanValue { get; set; }

    /// <summary>
    /// The maximum observed value of the variable within the time range.
    /// </summary>
    public float? MaxValue { get; set; }

    /// <summary>
    /// The minimum observed value of the variable within the time range.
    /// </summary>
    public float? MinValue { get; set; }

    /// <summary>
    /// Probability (0–1) that the variable reaches or exceeds extreme thresholds.
    /// </summary>
    [Range(0, 1)]
    public float ProbabilityExtreme { get; set; }

    /// <summary>
    /// Indicates the threshold or magnitude of the extreme condition (domain-specific meaning).
    /// </summary>
    public float ExtremeCondition { get; set; }

    /// <summary>
    /// Descriptive label for the analyzed time range (e.g., "2022-01-01 to 2022-12-31").
    /// </summary>
    [MaxLength(100)]
    public string? TimeRange { get; set; }

    /// <summary>
    /// Foreign key referencing the related <see cref="WeatherQuery"/>.
    /// </summary>
    [Required]
    public Guid QueryId { get; set; }

    /// <summary>
    /// Foreign key referencing the related <see cref="WeatherVariable"/>.
    /// </summary>
    [Required]
    public Guid VariableId { get; set; }

    /// <summary>
    /// Navigation property for the query associated with this result.
    /// </summary>
    [ForeignKey(nameof(QueryId))]
    public WeatherQuery Query { get; set; } = null!;

    /// <summary>
    /// Navigation property for the variable associated with this result.
    /// </summary>
    [ForeignKey(nameof(VariableId))]
    public WeatherVariable Variable { get; set; } = null!;
}
