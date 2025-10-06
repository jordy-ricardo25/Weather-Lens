using Newtonsoft.Json;
using Microsoft.ML.Data;
using Microsoft.ML;

namespace WeatherLens.Helpers;

/// <summary>
/// Provides static methods for predicting weather conditions
/// based on atmospheric and temporal variables.
/// </summary>
public static class WeatherPredictionHelper
{
    private sealed class InputData
    {
        [LoadColumn(0)]
        public string State { get; set; } = "";

        [LoadColumn(1)]
        public float Tair { get; set; }

        [LoadColumn(2)]
        public float Qair { get; set; }

        [LoadColumn(3)]
        public float Wind_N { get; set; }

        [LoadColumn(4)]
        public float Wind_E { get; set; }

        [LoadColumn(5)]
        public float SWdown { get; set; }

        [LoadColumn(6)]
        public float LWdown { get; set; }

        [LoadColumn(7)]
        public float Rainf { get; set; }
    }

    private sealed class FeatureData
    {
        public float Tair { get; set; }

        public float Qair { get; set; }

        public float WindSpeed { get; set; }

        public float SWdown { get; set; }

        public float LWdown { get; set; }

        public float SinHour { get; set; }

        public float CosHour { get; set; }

        public float SinDay { get; set; }

        public float CosDay { get; set; }

        public bool Label { get; set; }
    }

    private sealed class Prediction
    {
        [ColumnName("PredictedLabel")]
        public bool Predicted { get; set; }

        [ColumnName("Probability")]
        public float Probability { get; set; }

        [ColumnName("Score")]
        public float Score { get; set; }
    }

    /// <summary>
    /// Predicts the likelihood of precipitation based on input atmospheric parameters
    /// and temporal conditions using logistic regression.
    /// </summary>
    /// <summary>
    public static async Task<string> PredictWeatherAsync(
        DateTime date,
        double hour,
        double latitude,
        double longitude,
        double? temperature = null,
        double? humidity = null,
        double? windSpeed = null,
        double? shortwaveRadiation = null,
        double? longwaveRadiation = null,
        string datasetPath = "Assets/DataSeries.csv")
    {
        return await Task.Run(() =>
        {
            // === Step 1. Compute time-based trigonometric features ===
            var sinHour = Math.Sin(2 * Math.PI * hour / 24);
            var cosHour = Math.Cos(2 * Math.PI * hour / 24);
            var sinDay = Math.Sin(2 * Math.PI * date.DayOfYear / 365.0);
            var cosDay = Math.Cos(2 * Math.PI * date.DayOfYear / 365.0);

            // === Step 2. Define state centroids ===
            var centroids = new List<(string State, double Lat, double Lon)>
            {
                ("Alabama", 32.318231, -86.902298),
                ("Florida", 27.664827, -81.515754),
                ("Louisiana", 31.244823, -92.145024),
                ("Mississippi", 32.354668, -89.398528),
                ("Texas", 31.968599, -99.901813)
            };

            var closestState = centroids
                .OrderBy(c => Math.Sqrt(Math.Pow(c.Lat - latitude, 2) + Math.Pow(c.Lon - longitude, 2)))
                .First().State;

            // === Step 3. Load and filter dataset by state ===
            if (!File.Exists(datasetPath))
                throw new FileNotFoundException($"Dataset not found: {datasetPath}");

            var lines = File.ReadAllLines(datasetPath);
            var header = lines.First();
            var filteredLines = lines
                .Skip(1)
                .Where(l => l.StartsWith(closestState + ",", StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!filteredLines.Any())
                throw new InvalidOperationException($"No data found for state '{closestState}'.");

            var tempFile = Path.GetTempFileName();
            File.WriteAllLines(tempFile, new[] { header }.Concat(filteredLines));

            var mlContext = new MLContext();
            var data = mlContext.Data.LoadFromTextFile<InputData>(tempFile, hasHeader: true, separatorChar: ',');

            // === Step 4. Compute dataset averages for missing values ===
            var preview = mlContext.Data.CreateEnumerable<InputData>(data, reuseRowObject: false).ToList();
            double avgTemp = preview.Average(x => x.Tair);
            double avgHum = preview.Average(x => x.Qair);
            double avgWind = preview.Average(x => Math.Sqrt(x.Wind_N * x.Wind_N + x.Wind_E * x.Wind_E));
            double avgSw = preview.Average(x => x.SWdown);
            double avgLw = preview.Average(x => x.LWdown);

            // Fill missing inputs with averages
            double tair = temperature ?? avgTemp;
            double qair = humidity ?? avgHum;
            double wind = windSpeed ?? avgWind;
            double sw = shortwaveRadiation ?? avgSw;
            double lw = longwaveRadiation ?? avgLw;

            // === Step 5. Prepare and train model ===
            var pipeline = mlContext.Transforms.CustomMapping<InputData, FeatureData>((input, output) =>
            {
                output.Tair = input.Tair;
                output.Qair = input.Qair;
                output.SWdown = input.SWdown;
                output.LWdown = input.LWdown;
                output.WindSpeed = (float)Math.Sqrt(input.Wind_N * input.Wind_N + input.Wind_E * input.Wind_E);
                output.Label = input.Rainf > 0;
            }, contractName: "FeatureMapping")
            .Append(mlContext.Transforms.Concatenate("Features",
                nameof(FeatureData.Tair),
                nameof(FeatureData.Qair),
                nameof(FeatureData.WindSpeed),
                nameof(FeatureData.SWdown),
                nameof(FeatureData.LWdown)));

            var transformedData = pipeline.Fit(data).Transform(data);
            var trainer = mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression();
            var model = trainer.Fit(transformedData);

            // === Step 6. Create input sample (with defaults if needed) ===
            var input = new FeatureData
            {
                Tair = (float)tair,
                Qair = (float)qair,
                WindSpeed = (float)wind,
                SWdown = (float)sw,
                LWdown = (float)lw,
                SinHour = (float)sinHour,
                CosHour = (float)cosHour,
                SinDay = (float)sinDay,
                CosDay = (float)cosDay
            };

            var predictor = mlContext.Model.CreatePredictionEngine<FeatureData, Prediction>(model);
            var prediction = predictor.Predict(input);

            // === Step 7. Classify result ===
            var label = prediction.Probability switch
            {
                <= 0.25f => "Clear sky",
                <= 0.50f => "Partly cloudy",
                <= 0.75f => "Chance of rain",
                _ => "High rain or storm"
            };

            var result = new
            {
                state = closestState,
                probability = prediction.Probability,
                classification = label
            };

            try { File.Delete(tempFile); } catch { }

            return JsonConvert.SerializeObject(result, Formatting.Indented);
        });
    }
}
