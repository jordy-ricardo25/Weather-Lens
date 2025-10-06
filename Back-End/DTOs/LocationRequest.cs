using System.ComponentModel.DataAnnotations;

namespace WeatherLens.DTOs;

/// <summary>
/// Represents the data required to create or update a location.
/// </summary>
public sealed class LocationRequest
{
    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required, Range(-90, 90)]
    public float Latitude { get; set; }

    [Required, Range(-180, 180)]
    public float Longitude { get; set; }
}
