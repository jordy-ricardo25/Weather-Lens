using Xtractor.Clients;
using Xtractor.Helpers;

// Define the NASA variables to extract
var variables = new Dictionary<string, string>
{
    { "Rainf", "Rainf (Precipitation) [kg/m²/s]" },
    { "Tair", "Tair (Air temperature at 2 m) [K]" },
    { "Qair", "Qair (Specific humidity) [kg/kg]" },
    { "Wind_N", "Wind_N (Northward wind component) [m/s]" },
    { "Wind_E", "Wind_E (Eastward wind component) [m/s]" },
    { "SWdown", "SWdown (Downward shortwave radiation) [W/m²]" },
    { "LWdown", "LWdown (Downward longwave radiation) [W/m²]" }
};

// Time range for the dataset
var startDate = "2020-01-01T00";
var endDate = "2025-01-01T00";

// Output directory
var outputDir = "./Output";
Directory.CreateDirectory(outputDir);

try
{
    // Load U.S. states and coordinates from CSV
    var states = await CsvHelper.ReadCsvAsync("./Assets/USA_States.csv");

    foreach (var state in states)
    {
        Console.WriteLine($"\n========== 🌎 {state.State} ==========\n");

        // Dictionary: DateTime → values by variable
        var mergedData = new Dictionary<string, Dictionary<string, double?>>();

        foreach (var kv in variables)
        {
            var variable = kv.Key;

            try
            {
                // Fetch NASA GES DISC time-series data
                var data = await NasaClient.FetchTimeSeriesAsync(state, variable, startDate, endDate);
                Console.WriteLine($"✅ {variable}: {data.Count} records retrieved");

                // Merge all variables by date
                foreach (var row in data)
                {
                    if (!mergedData.ContainsKey(row.DateTime))
                        mergedData[row.DateTime] = new Dictionary<string, double?>();

                    mergedData[row.DateTime][variable] = row.Value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error fetching {variable}: {ex.Message}");
            }
        }

        // Write merged data to CSV file
        var outputPath = Path.Combine(outputDir, $"{state.State.Replace(" ", "_")}.csv");
        await CsvHelper.WriteMergedCsvAsync(outputPath, mergedData, variables.Keys.ToList());
        Console.WriteLine($"\n💾 Saved: {outputPath}\n");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Global error: {ex.Message}");
}
