namespace WeatherLens.DTOs;

/// <summary>
/// Represents computed weather result data returned by the API.
/// </summary>
public sealed class WeatherResultResult
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public float ProbabilityExtreme { get; set; }
    public string ExtremeCondition { get; set; } = string.Empty;
    public Guid QueryId { get; set; }
    public Guid VariableId { get; set; }
}
