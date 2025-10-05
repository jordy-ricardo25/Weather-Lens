namespace Weather_Lens.Models;

public sealed class Location
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Name { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public string? Country { get; set; }
    public string? Region { get; set; }
}
