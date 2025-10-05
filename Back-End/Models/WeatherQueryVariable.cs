using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeatherLens.Models;

/// <summary>
/// Represents the relationship between a weather query and a specific weather variable.
/// </summary>
[Table("weather_query_variables")]
public sealed class WeatherQueryVariable
{
    /// <summary>
    /// Unique identifier for the query-variable relationship (primary key).
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Date and time when the record was created (UTC).
    /// </summary>
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Foreign key referencing the associated <see cref="WeatherQuery"/>.
    /// </summary>
    [Required]
    public Guid QueryId { get; set; }

    /// <summary>
    /// Foreign key referencing the associated <see cref="WeatherVariable"/>.
    /// </summary>
    [Required]
    public Guid VariableId { get; set; }

    /// <summary>
    /// Navigation property for the weather query related to this record.
    /// </summary>
    [ForeignKey(nameof(QueryId))]
    public WeatherQuery Query { get; set; } = null!;

    /// <summary>
    /// Navigation property for the weather variable included in this query.
    /// </summary>
    [ForeignKey(nameof(VariableId))]
    public WeatherVariable Variable { get; set; } = null!;
}
