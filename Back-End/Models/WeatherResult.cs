namespace Weather_Lens.Models;

public sealed class WeatherResult
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public float? MeanValue { get; set; }
    public float? MaxValue { get; set; }
    public float? MinValue { get; set; }
    public float ProbabilityExtreme { get; set; }
    public float ExtremeCondition { get; set; }
    public string? TimeRange { get; set; }
    public Guid QueryId { get; set; }
    public Guid VariableId { get; set; }
    public WeatherQuery Query { get; set; }
    public WeatherVariable Variable { get; set; }
}
