namespace Xtractor.Models;

/// <summary>
/// Represents a geographic location within the United States,
/// including its state name and corresponding latitude/longitude coordinates.
/// </summary>
public sealed class Location
{
    /// <summary>
    /// The name of the state (e.g., "California", "Texas").
    /// </summary>
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// The latitude coordinate of the state’s reference point,
    /// expressed as a string using the invariant culture (dot as decimal separator).
    /// </summary>
    public string Latitude { get; set; } = string.Empty;

    /// <summary>
    /// The longitude coordinate of the state’s reference point,
    /// expressed as a string using the invariant culture (dot as decimal separator).
    /// </summary>
    public string Longitude { get; set; } = string.Empty;
}
