using System.ComponentModel.DataAnnotations;

namespace WeatherLens.DTOs;

/// <summary>
/// Represents the data required to link a weather query with a variable.
/// </summary>
public sealed class WeatherQueryVariableRequest
{
    [Required]
    public Guid QueryId { get; set; }

    [Required]
    public Guid VariableId { get; set; }
}
