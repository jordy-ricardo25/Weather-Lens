using Microsoft.AspNetCore.Mvc;
using WeatherLens.Data.Repositories;
using WeatherLens.Helpers;
using WeatherLens.Models;
using WeatherLens.DTOs;
using AutoMapper;

namespace WeatherLens.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PredictionsController : ControllerBase
{
    private readonly IRepository<Location> _repositoryLocations;
    private readonly IRepository<WeatherQuery> _repositoryQueries;
    private readonly IRepository<WeatherVariable> _repositoryVariables;
    private readonly IRepository<WeatherQueryVariable> _repositoryQueryVariables;
    private readonly IRepository<WeatherResult> _repositoryResults;
    private readonly IMapper _mapper;

    public PredictionsController(
        IRepository<Location> repositoryLocations,
        IRepository<WeatherQuery> repositoryQueries,
        IRepository<WeatherVariable> repositoryVariables,
        IRepository<WeatherQueryVariable> repositoryQueryVariables,
        IRepository<WeatherResult> repositoryResults,
        IMapper mapper)
    {
        _repositoryLocations = repositoryLocations;
        _repositoryQueries = repositoryQueries;
        _repositoryVariables = repositoryVariables;
        _repositoryQueryVariables = repositoryQueryVariables;
        _repositoryResults = repositoryResults;
        _mapper = mapper;
    }

    /// <summary>
    /// Genera una predicción meteorológica basada en fecha y coordenadas.
    /// </summary>
    /// <param name="date">Fecha y hora de la consulta.</param>
    /// <param name="latitude">Latitud del punto de consulta.</param>
    /// <param name="longitude">Longitud del punto de consulta.</param>
    [HttpGet("predict")]
    [ProducesResponseType(typeof(WeatherResultResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WeatherResultResult>> GetPrediction(
        [FromQuery] DateTime date,
        [FromQuery] float latitude,
        [FromQuery] float longitude)
    {
        if (latitude is < -90 or > 90 || longitude is < -180 or > 180)
            return BadRequest("Coordenadas fuera de rango.");

        // Guardar ubicación
        var location = await _repositoryLocations.AddAsync(new Location
        {
            Name = $"Lat:{latitude:F2}, Lon:{longitude:F2}",
            Latitude = latitude,
            Longitude = longitude
        });

        // Guardar consulta
        var query = await _repositoryQueries.AddAsync(new WeatherQuery
        {
            Date = date,
            LocationId = location.Id
        });

        // Llamar al modelo de predicción
        var prediction = await WeatherPredictionHelper.PredictWeatherAsync(
            date.Date,
            date.Hour,
            latitude,
            longitude
        );

        //// Guardar resultado
        //var result = await _repositoryResults.AddAsync(new WeatherResult
        //{
        //    QueryId = query.Id,
        //    PredictedCondition = prediction.Condition,
        //    Confidence = prediction.Confidence,
        //    CreatedAt = DateTime.UtcNow
        //});

        //// Mapear a DTO para devolver
        //var resultDto = _mapper.Map<WeatherResultDto>(result);
        return Ok(new WeatherResultResult());
    }
}
