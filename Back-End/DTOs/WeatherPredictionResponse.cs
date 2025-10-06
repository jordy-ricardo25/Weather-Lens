namespace WeatherLens.DTOs;

public sealed class WeatherPredictionResponse
{
    public float Probability { get; set; }
    public string Prediction { get; set; } = "";
}
