namespace WeatherLens.DTOs;

public sealed class WeatherPredictionRequest
{
    public DateTime Date { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
}
