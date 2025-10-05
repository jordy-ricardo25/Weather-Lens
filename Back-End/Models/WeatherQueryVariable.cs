using System;
namespace Weather_Lens.Models;

public sealed class WeatherQueryVariable
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid QueryId { get; set; }
    public Guid VariableId { get; set; }
    public WeatherQuery Query { get; set; }
    public WeatherVariable Variable { get; set; }
}
