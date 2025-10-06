using Microsoft.AspNetCore.Mvc;
using WeatherLens.Data.Repositories;
using WeatherLens.Models;
using WeatherLens.DTOs;
using AutoMapper;

namespace WeatherLens.Controllers;

/// <summary>
/// Provides endpoints for managing geographic locations within the WeatherLens system.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class LocationsController : ControllerBase
{
    private readonly IRepository<Location> _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="LocationsController"/> class.
    /// </summary>
    /// <param name="repository">Repository used to manage location persistence.</param>
    /// <param name="mapper">AutoMapper instance for DTO conversions.</param>
    public LocationsController(IRepository<Location> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all locations registered in the system.
    /// </summary>
    /// <returns>A list of <see cref="LocationResult"/> objects.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LocationResult>>> GetAll()
    {
        var locations = await _repository.GetAllAsync();
        var results = _mapper.Map<IEnumerable<LocationResult>>(locations);
        return Ok(results);
    }

    /// <summary>
    /// Retrieves a specific location by unique identifier.
    /// </summary>
    /// <param name="id">The location’s unique identifier.</param>
    /// <returns>The corresponding <see cref="LocationResult"/> if found.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LocationResult>> GetById(Guid id)
    {
        var location = await _repository.GetByIdAsync(id);
        if (location is null) return NotFound();

        var result = _mapper.Map<LocationResult>(location);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new location record.
    /// </summary>
    /// <param name="request">The <see cref="LocationRequest"/> object containing the location data.</param>
    /// <returns>The created <see cref="LocationResult"/>.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LocationResult>> Create([FromBody] LocationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = _mapper.Map<Location>(request);
        var created = await _repository.AddAsync(entity);
        var result = _mapper.Map<LocationResult>(created);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Updates an existing location record.
    /// </summary>
    /// <param name="id">The ID of the location to update.</param>
    /// <param name="request">The updated location data.</param>
    /// <returns>The updated <see cref="LocationResult"/>.</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LocationResult>> Update(Guid id, [FromBody] LocationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return NotFound();

        _mapper.Map(request, existing);
        var updated = await _repository.UpdateAsync(existing);
        var result = _mapper.Map<LocationResult>(updated);

        return Ok(result);
    }

    /// <summary>
    /// Deletes a location by unique identifier.
    /// </summary>
    /// <param name="id">The ID of the location to delete.</param>
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
