using System.ComponentModel.DataAnnotations;

namespace WeatherLens.DTOs;

/// <summary>
/// Represents the analytical data sent to store results of a weather query.
/// </summary>
public sealed class WeatherResultRequest
{
    public float? MeanValue { get; set; }
    public float? MaxValue { get; set; }
    public float? MinValue { get; set; }

    [Range(0, 1)]
    public float ProbabilityExtreme { get; set; }

    public string ExtremeCondition { get; set; } = string.Empty;

    [Required]
    public Guid QueryId { get; set; }

    [Required]
    public Guid VariableId { get; set; }
}
