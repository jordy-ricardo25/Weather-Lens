namespace Xtractor.Models;

/// <summary>
/// Represents a single time-series data point retrieved from a NASA dataset,
/// including a timestamp and its corresponding numeric value.
/// </summary>
public class TimeData
{
    /// <summary>
    /// The timestamp of the observation (in ISO 8601 format, e.g. "2020-01-01T00").
    /// </summary>
    public string DateTime { get; set; } = string.Empty;

    /// <summary>
    /// The measured or calculated value associated with the timestamp.
    /// </summary>
    /// <remarks>
    /// This value may be <see langword="null"/> if the dataset contains missing or invalid entries.
    /// </remarks>
    public double? Value { get; set; }
}
