using System.ComponentModel.DataAnnotations;

namespace WeatherLens.DTOs;

/// <summary>
/// Represents the data required to submit a new weather query.
/// </summary>
public sealed class WeatherQueryRequest
{
    [Required]
    public DateTime Date { get; set; }

    [Required]
    public Guid LocationId { get; set; }
}
