namespace WeatherLens.DTOs;

/// <summary>
/// Represents the weather variable data returned by the API.
/// </summary>
public sealed class WeatherVariableResult
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
