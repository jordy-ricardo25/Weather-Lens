namespace WeatherLens.DTOs;

/// <summary>
/// Represents the location data returned by the API.
/// </summary>
public sealed class LocationResult
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Name { get; set; } = string.Empty;
    public float Latitude { get; set; }
    public float Longitude { get; set; }
}
