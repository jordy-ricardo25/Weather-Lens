using System.Globalization;
using Xtractor.Models;

namespace Xtractor.Clients;

/// <summary>
/// Provides methods to fetch time-series climate and hydrology data
/// from NASA GES DISC (Goddard Earth Sciences Data and Information Services Center).
/// </summary>
public static class NasaClient
{
    private const string BASE_URL = "https://hydro1.gesdisc.eosdis.nasa.gov/daac-bin/access/timeseries.cgi";
    private const string TYPE = "asc2";

    private static readonly HttpClient http = new();

    /// <summary>
    /// Fetches hourly or daily time-series data from NASA GES DISC for a specific
    /// geographic location (latitude/longitude) and variable.
    /// </summary>
    /// <param name="state">The <see cref="Location"/> object containing the state name and coordinates.</param>
    /// <param name="variable">The variable short name (e.g., <c>Rainf</c>, <c>Tair</c>, <c>SWdown</c>).</param>
    /// <param name="startDate">The start date in ISO format (e.g., <c>2020-01-01T00</c>).</param>
    /// <param name="endDate">The end date in ISO format (e.g., <c>2020-06-01T00</c>).</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation.
    /// The task result contains a list of <see cref="TimeData"/> objects with date and value fields.
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown if:
    /// <list type="bullet">
    /// <item><description>The latitude or longitude values are invalid.</description></item>
    /// <item><description>The NASA endpoint returns no data for the specified location and variable.</description></item>
    /// </list>
    /// </exception>
    /// <remarks>
    /// The function queries the NASA GES DISC <c>timeseries.cgi</c> endpoint, which supports
    /// variables from multiple datasets such as NLDAS, GLDAS, and others.  
    /// Data is returned in ASCII format and parsed line by line into structured time-series entries.
    /// </remarks>
    public static async Task<List<TimeData>> FetchTimeSeriesAsync(Location state, string variable, string startDate, string endDate)
    {
        // Validate coordinates
        if (!double.TryParse(state.Latitude, NumberStyles.Any, CultureInfo.InvariantCulture, out var lat) ||
            !double.TryParse(state.Longitude, NumberStyles.Any, CultureInfo.InvariantCulture, out var lon))
        {
            throw new Exception($"⚠️ Invalid coordinates for {state.State}: {state.Latitude}, {state.Longitude}");
        }

        // Build request URL
        var url =
            $"{BASE_URL}?variable=NLDAS2:NLDAS_FORA0125_H_v2.0:{variable}" +
            $"&type={TYPE}" +
            $"&location=GEOM:POINT({lon}, {lat})" +
            $"&startDate={startDate}" +
            $"&endDate={endDate}";

        Console.WriteLine($"🌎 Fetching data for {state.State} ({lat}, {lon})...");

        // Configure headers
        http.DefaultRequestHeaders.Clear();
        http.DefaultRequestHeaders.Add("User-Agent", "WeatherLensExtractor/.NET6");

        // Send request
        var response = await http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        // Parse response body
        string body = await response.Content.ReadAsStringAsync();
        var lines = body.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        // Locate header line
        int startIndex = Array.FindIndex(lines, l => l.StartsWith("Date&Time", StringComparison.OrdinalIgnoreCase));
        if (startIndex == -1)
            throw new Exception($"⚠️ No data available for {state.State}");

        // Parse time-series values
        var result = new List<TimeData>();
        for (int i = startIndex + 1; i < lines.Length; i++)
        {
            var parts = lines[i].Split('\t', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) continue;

            double? value = null;
            if (parts.Length > 1 && double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed))
                value = parsed;

            result.Add(new TimeData
            {
                DateTime = parts[0],
                Value = value
            });
        }

        return result;
    }
}
