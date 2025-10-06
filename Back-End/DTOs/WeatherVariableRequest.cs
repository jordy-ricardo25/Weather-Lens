using System.ComponentModel.DataAnnotations;

namespace WeatherLens.DTOs;

/// <summary>
/// Represents the data required to register or update a weather variable.
/// </summary>
public sealed class WeatherVariableRequest
{
    [Required, MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(30)]
    public string Unit { get; set; } = string.Empty;

    [Required, MaxLength(255)]
    public string Description { get; set; } = string.Empty;
}
