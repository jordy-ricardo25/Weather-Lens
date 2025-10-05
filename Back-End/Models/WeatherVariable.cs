using System;
namespace Weather_Lens.Models;

public sealed class WeatherVariable
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Name { get; set; }
    public string Unit { get; set; }
    public string Description { get; set; }
    public string DataUrl { get; set; }
}
