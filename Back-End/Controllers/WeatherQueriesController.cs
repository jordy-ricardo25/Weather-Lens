using Microsoft.AspNetCore.Mvc;
using WeatherLens.Data.Repositories;
using WeatherLens.Models;
using WeatherLens.DTOs;
using AutoMapper;

namespace WeatherLens.Controllers;

/// <summary>
/// Provides endpoints for managing user-submitted weather data queries.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class WeatherQueriesController : ControllerBase
{
    private readonly IRepository<WeatherQuery> _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="WeatherQueriesController"/> class.
    /// </summary>
    /// <param name="repository">Repository for managing weather queries.</param>
    /// <param name="mapper">AutoMapper instance for DTO conversions.</param>
    public WeatherQueriesController(IRepository<WeatherQuery> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all weather queries in the system, including related user and location data.
    /// </summary>
    /// <returns>A list of <see cref="WeatherQueryResult"/> objects.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<WeatherQueryResult>>> GetAll()
    {
        var queries = await _repository.GetAllAsync();
        var results = _mapper.Map<IEnumerable<WeatherQueryResult>>(queries);
        return Ok(results);
    }

    /// <summary>
    /// Retrieves a specific weather query by its unique identifier.
    /// </summary>
    /// <param name="id">The unique ID of the query.</param>
    /// <returns>The corresponding <see cref="WeatherQueryResult"/> if found.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WeatherQueryResult>> GetById(Guid id)
    {
        var query = await _repository.GetByIdAsync(id);
        if (query is null) return NotFound();

        var result = _mapper.Map<WeatherQueryResult>(query);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new weather query record.
    /// </summary>
    /// <param name="request">The <see cref="WeatherQueryRequest"/> object containing the query details.</param>
    /// <returns>The created <see cref="WeatherQueryResult"/>.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WeatherQueryResult>> Create([FromBody] WeatherQueryRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = _mapper.Map<WeatherQuery>(request);
        var created = await _repository.AddAsync(entity);
        var result = _mapper.Map<WeatherQueryResult>(created);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Updates an existing weather query record.
    /// </summary>
    /// <param name="id">The ID of the query to update.</param>
    /// <param name="request">The updated query data.</param>
    /// <returns>The updated <see cref="WeatherQueryResult"/>.</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WeatherQueryResult>> Update(Guid id, [FromBody] WeatherQueryRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return NotFound();

        _mapper.Map(request, existing);
        var updated = await _repository.UpdateAsync(existing);
        var result = _mapper.Map<WeatherQueryResult>(updated);

        return Ok(result);
    }

    /// <summary>
    /// Deletes a weather query by unique identifier.
    /// </summary>
    /// <param name="id">The ID of the query to delete.</param>
    /// <returns>A 204 No Content response if successful.</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _repository.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
