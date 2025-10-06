using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeatherLens.Entities;

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
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Timestamp indicating when this result record was created (UTC).
    /// </summary>
    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Probability (0–1) that the variable reaches or exceeds extreme thresholds.
    /// </summary>
    [Range(0, 1)]
    [Column("probability_extreme")]
    public float ProbabilityExtreme { get; set; }

    /// <summary>
    /// Indicates the threshold or magnitude of the extreme condition (domain-specific meaning).
    /// </summary>
    [Column("extreme_condition")]
    public string ExtremeCondition { get; set; } = string.Empty;

    /// <summary>
    /// Foreign key referencing the related <see cref="WeatherQuery"/>.
    /// </summary>
    [Required]
    [Column("query_id")]
    public Guid QueryId { get; set; }

    /// <summary>
    /// Foreign key referencing the related <see cref="WeatherVariable"/>.
    /// </summary>
    [Required]
    [Column("variable_id")]
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
