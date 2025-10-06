namespace WeatherLens.DTOs;

/// <summary>
/// Represents the weather query data returned by the API.
/// </summary>
public sealed class WeatherQueryResult
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime Date { get; set; }
    public Guid LocationId { get; set; }
}
