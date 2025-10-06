using WeatherLens.Entities;
using WeatherLens.DTOs;
using WeatherLens.Data;
using Microsoft.ML.Data;
using Microsoft.ML;
using Microsoft.EntityFrameworkCore;

namespace WeatherLens.Services;

/// <summary>
/// Provides static methods for predicting weather conditions
/// based on atmospheric and temporal variables.
/// </summary>
public sealed class WeatherPredictionService : IWeatherPredictionService
{
    private readonly AppDbContext _context;

    public WeatherPredictionService(AppDbContext context)
    {
        _context = context;
    }

    // === 1️⃣ InputData ===
    // Coincide exactamente con los encabezados del CSV (minúsculas, en español)
    private sealed class InputData
    {
        [LoadColumn(0)] public string estado { get; set; } = "";
        [LoadColumn(1)] public float latitud { get; set; }
        [LoadColumn(2)] public float longitud { get; set; }
        [LoadColumn(3)] public float año { get; set; }
        [LoadColumn(4)] public float mes { get; set; }
        [LoadColumn(5)] public float hora_del_dia { get; set; }
        [LoadColumn(6)] public float precipitacion { get; set; }
        [LoadColumn(7)] public float temperatura { get; set; }
        [LoadColumn(8)] public float humedad { get; set; }
        [LoadColumn(9)] public float viento_n { get; set; }
        [LoadColumn(10)] public float viento_e { get; set; }
        [LoadColumn(11)] public float radiacion_corta { get; set; }
        [LoadColumn(12)] public float radiacion_larga { get; set; }
        [LoadColumn(13)] public float sin_hora { get; set; }
        [LoadColumn(14)] public float cos_hora { get; set; }
        [LoadColumn(15)] public float sin_dia { get; set; }
        [LoadColumn(16)] public float cos_dia { get; set; }
    }

    // === 2️⃣ FeatureData ===
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

    // === 3️⃣ Prediction ===
    private sealed class Prediction
    {
        [ColumnName("PredictedLabel")] public bool Predicted { get; set; }
        [ColumnName("Probability")] public float Probability { get; set; }
        [ColumnName("Score")] public float Score { get; set; }
    }

    // === 4️⃣ Método principal ===
    public async Task<WeatherPredictionResponse> PredictWeatherAsync(
        DateTime date,
        float latitude,
        float longitude,
        double? temperature = null,
        double? humidity = null,
        double? windSpeed = null,
        double? shortwaveRadiation = null,
        double? longwaveRadiation = null,
        string datasetPath = "Assets/DataSeries.csv")
    {
        // === Crear registros de ubicación y consulta ===
        var location = new Location
        {
            Name = "",
            Latitude = latitude,
            Longitude = longitude
        };
        _context.Locations.Add(location);
        await _context.SaveChangesAsync();

        var query = new WeatherQuery
        {
            Date = date,
            LocationId = location.Id
        };
        _context.Queries.Add(query);
        await _context.SaveChangesAsync();

        // === Calcular características temporales ===
        var sinHour = Math.Sin(2 * Math.PI * date.Hour / 24);
        var cosHour = Math.Cos(2 * Math.PI * date.Hour / 24);
        var sinDay = Math.Sin(2 * Math.PI * date.DayOfYear / 365.0);
        var cosDay = Math.Cos(2 * Math.PI * date.DayOfYear / 365.0);

        // === Buscar estado más cercano ===
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

        // === Cargar dataset y limpiar comillas del encabezado ===
        if (!File.Exists(datasetPath))
            throw new FileNotFoundException($"Dataset not found: {datasetPath}");

        var rawLines = File.ReadAllLines(datasetPath);

        // 🔧 Eliminar comillas del encabezado
        if (rawLines.Length > 0)
            rawLines[0] = rawLines[0].Replace("\"", "");

        // 🔧 Guardar un archivo temporal limpio
        var tempFile = Path.GetTempFileName();
        File.WriteAllLines(tempFile, rawLines);

        var mlContext = new MLContext();

        // === Loader sin comillas en los nombres ===
        var textLoader = mlContext.Data.CreateTextLoader(
            columns: new[]
            {
        new TextLoader.Column("estado", DataKind.String, 0),
        new TextLoader.Column("latitud", DataKind.Single, 1),
        new TextLoader.Column("longitud", DataKind.Single, 2),
        new TextLoader.Column("año", DataKind.Single, 3),
        new TextLoader.Column("mes", DataKind.Single, 4),
        new TextLoader.Column("hora_del_dia", DataKind.Single, 5),
        new TextLoader.Column("precipitacion", DataKind.Single, 6),
        new TextLoader.Column("temperatura", DataKind.Single, 7),
        new TextLoader.Column("humedad", DataKind.Single, 8),
        new TextLoader.Column("viento_n", DataKind.Single, 9),
        new TextLoader.Column("viento_e", DataKind.Single, 10),
        new TextLoader.Column("radiacion_corta", DataKind.Single, 11),
        new TextLoader.Column("radiacion_larga", DataKind.Single, 12),
        new TextLoader.Column("sin_hora", DataKind.Single, 13),
        new TextLoader.Column("cos_hora", DataKind.Single, 14),
        new TextLoader.Column("sin_dia", DataKind.Single, 15),
        new TextLoader.Column("cos_dia", DataKind.Single, 16)
            },
            hasHeader: true,
            separatorChar: ',',
            allowQuoting: true,
            trimWhitespace: true
        );

        var data = textLoader.Load(tempFile);

        // === DEBUG: Mostrar las columnas reales que detecta ML.NET ===
        Console.WriteLine("🔍 Columnas detectadas por ML.NET:");
        foreach (var col in data.Schema)
        {
            Console.WriteLine($"  • {col.Name} ({col.Type})");
        }

        // === Calcular promedios para valores faltantes ===
        var preview = mlContext.Data.CreateEnumerable<InputData>(data, reuseRowObject: false).ToList();

        double avgTemp = preview.Average(x => x.temperatura);
        double avgHum = preview.Average(x => x.humedad);
        double avgWind = preview.Average(x => Math.Sqrt(x.viento_n * x.viento_n + x.viento_e * x.viento_e));
        double avgSw = preview.Average(x => x.radiacion_corta);
        double avgLw = preview.Average(x => x.radiacion_larga);

        double tair = temperature ?? avgTemp;
        double qair = humidity ?? avgHum;
        double wind = windSpeed ?? avgWind;
        double sw = shortwaveRadiation ?? avgSw;
        double lw = longwaveRadiation ?? avgLw;

        // === Definir pipeline ===
        var pipeline = mlContext.Transforms.CustomMapping<InputData, FeatureData>((input, output) =>
        {
            output.Tair = input.temperatura;
            output.Qair = input.humedad;
            output.SWdown = input.radiacion_corta;
            output.LWdown = input.radiacion_larga;
            output.WindSpeed = (float)Math.Sqrt(input.viento_n * input.viento_n + input.viento_e * input.viento_e);
            output.Label = input.precipitacion > 0;
        }, contractName: "FeatureMapping")
        .Append(mlContext.Transforms.Concatenate("Features",
            nameof(FeatureData.Tair),
            nameof(FeatureData.Qair),
            nameof(FeatureData.WindSpeed),
            nameof(FeatureData.SWdown),
            nameof(FeatureData.LWdown)))
        .Append(mlContext.BinaryClassification.Trainers
            .LbfgsLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));

        // === Entrenar modelo ===
        var model = pipeline.Fit(data);

        // === Crear muestra y predecir ===
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

        // === Interpretar resultado ===
        var label = prediction.Probability switch
        {
            <= 0.25f => "Clear sky",
            <= 0.50f => "Partly cloudy",
            <= 0.75f => "Chance of rain",
            _ => "High rain or storm"
        };

        try { File.Delete(tempFile); } catch { }

        // === Registrar resultado ===
        var rainVariable = await _context.Variables.FirstOrDefaultAsync(v => v.Name == "Rainf");
        if (rainVariable != null)
        {
            var result = new WeatherResult
            {
                QueryId = query.Id,
                VariableId = rainVariable.Id,
                ProbabilityExtreme = prediction.Probability,
                ExtremeCondition = label
            };
            _context.Results.Add(result);
            await _context.SaveChangesAsync();
        }

        // === Respuesta final ===
        return new WeatherPredictionResponse
        {
            Prediction = label,
            Probability = prediction.Probability
        };
    }

    public Task<WeatherFeatureSet> GetFeatureSetAsync(DateTime date, float latitude, float longitude)
    {
        throw new NotImplementedException();
    }
}
