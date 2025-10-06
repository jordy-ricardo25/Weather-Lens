using WeatherLens.DTOs;

namespace WeatherLens.Services;

public interface IWeatherPredictionService
{
    public Task<WeatherPredictionResponse> PredictWeatherAsync(
        DateTime date,
        float latitude,
        float longitude,
        double? temperature = null,
        double? humidity = null,
        double? windSpeed = null,
        double? shortwaveRadiation = null,
        double? longwaveRadiation = null,
        string datasetPath = "Assets/DataSeries.csv");

    public Task<WeatherFeatureSet> GetFeatureSetAsync(DateTime date, float latitude, float longitude);
}
