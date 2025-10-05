using System.Globalization;
using System.Text;
using Xtractor.Models;

namespace Xtractor.Helpers;

/// <summary>
/// Provides helper methods for reading and writing CSV files,
/// including parsing state location data and writing merged datasets.
/// </summary>
public static class CsvHelper
{
    /// <summary>
    /// Reads a CSV file with the structure: <c>State;Latitude;Longitude</c>.
    /// </summary>
    /// <param name="filePath">The path to the CSV file to read.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation.
    /// The task result contains a list of <see cref="Location"/> objects parsed from the CSV file.
    /// </returns>
    /// <remarks>
    /// - The CSV file must use a semicolon (<c>;</c>) as the field delimiter.  
    /// - The first row is assumed to be a header and will be skipped automatically.  
    /// - Empty or invalid lines are ignored.  
    /// </remarks>
    public static async Task<List<Location>> ReadCsvAsync(string filePath)
    {
        var lines = await File.ReadAllLinesAsync(filePath, Encoding.UTF8);
        var result = new List<Location>();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = line.Split(';', StringSplitOptions.TrimEntries);
            if (parts.Length < 3 || parts[0].Equals("Estado", StringComparison.OrdinalIgnoreCase))
                continue;

            result.Add(new Location
            {
                State = parts[0],
                Latitude = parts[1],
                Longitude = parts[2]
            });
        }

        return result;
    }

    /// <summary>
    /// Writes a merged dataset to a CSV file, combining multiple variables for each date.
    /// </summary>
    /// <param name="filePath">The output file path where the CSV will be written.</param>
    /// <param name="data">
    /// A nested dictionary where:
    /// <list type="bullet">
    /// <item><description>The first key represents the date/time (string).</description></item>
    /// <item><description>The inner dictionary maps variable names to their corresponding numeric values.</description></item>
    /// </list>
    /// </param>
    /// <param name="keys">A list of variable names that defines the column order.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <remarks>
    /// - The first line of the file will contain the column headers: <c>Date&amp;Time;Var1;Var2;...</c>.  
    /// - The rows are ordered chronologically by the date key.  
    /// - Missing values are written as empty fields.  
    /// - Numeric values are formatted using invariant culture to ensure dot (<c>.</c>) as the decimal separator.  
    /// </remarks>
    public static async Task WriteMergedCsvAsync(string filePath, Dictionary<string, Dictionary<string, double?>> data, List<string> keys)
    {
        var sb = new StringBuilder();

        // Header
        sb.Append("Date&Time");
        foreach (var variableName in keys)
            sb.Append($";{variableName}");
        sb.AppendLine();

        // Sort and write rows by date
        foreach (var entry in data.OrderBy(e => e.Key))
        {
            var date = entry.Key;
            sb.Append(date);

            foreach (var variableName in keys)
            {
                var value = entry.Value.ContainsKey(variableName)
                    ? entry.Value[variableName]?.ToString(CultureInfo.InvariantCulture)
                    : string.Empty;
                sb.Append($";{value}");
            }

            sb.AppendLine();
        }

        await File.WriteAllTextAsync(filePath, sb.ToString(), Encoding.UTF8);
    }
}
