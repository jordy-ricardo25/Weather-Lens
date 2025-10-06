namespace WeatherLens.DTOs;

/// <summary>
/// Represents the relationship between a query and a variable returned by the API.
/// </summary>
public sealed class WeatherQueryVariableResult
{
    public Guid Id { get; set; }
    public Guid QueryId { get; set; }
    public Guid VariableId { get; set; }
    public DateTime CreatedAt { get; set; }
}
