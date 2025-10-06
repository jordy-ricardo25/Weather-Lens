using Microsoft.AspNetCore.Mvc;
using WeatherLens.Services;
using WeatherLens.DTOs;

namespace WeatherLens.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PredictionsController : ControllerBase
{
    private readonly IWeatherPredictionService _service;

    public PredictionsController(IWeatherPredictionService service)
    {
        _service = service;
    }

    /// <summary>
    /// Genera una predicción meteorológica basada en fecha y coordenadas.
    /// </summary>
    /// <param name="date">Fecha y hora de la consulta.</param>
    /// <param name="latitude">Latitud del punto de consulta.</param>
    /// <param name="longitude">Longitud del punto de consulta.</param>
    [HttpPost]
    public async Task<ActionResult<WeatherPredictionResponse>> GetPrediction([FromBody] WeatherPredictionRequest request)
    {
        try
        {
            var result = await _service.PredictWeatherAsync(request.Date, request.Latitude, request.Longitude);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
