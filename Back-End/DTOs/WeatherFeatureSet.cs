namespace WeatherLens.DTOs;

/// <summary>
/// Represents the set of NASA variables at a given location and date.
/// </summary>
public sealed class WeatherFeatureSet
{
    public float Tair { get; set; }
    public float Qair { get; set; }
    public float Wind_N { get; set; }
    public float Wind_E { get; set; }
    public float SWdown { get; set; }
    public float LWdown { get; set; }
    public float Rainf { get; set; }
}
