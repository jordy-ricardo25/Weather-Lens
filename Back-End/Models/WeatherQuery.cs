using System;
namespace Weather_Lens.Models;

public sealed class WeatherQuery
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime Date { get; set; }
    public string? Status { get; set; }
    public Guid UserId { get; set; }
    public Guid LocationId { get; set; }
    public User User { get; set; }
    public Location Location { get; set; }
}
