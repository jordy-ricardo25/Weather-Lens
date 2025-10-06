using Microsoft.AspNetCore.Mvc;
using WeatherLens.Data.Repositories;
using WeatherLens.Models;
using WeatherLens.DTOs;
using AutoMapper;

namespace WeatherLens.Controllers;

/// <summary>
/// Provides endpoints for managing and retrieving computed weather results.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class WeatherResultsController : ControllerBase
{
    private readonly IRepository<WeatherResult> _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="WeatherResultsController"/> class.
    /// </summary>
    /// <param name="repository">Repository for managing weather results.</param>
    /// <param name="mapper">AutoMapper instance for DTO conversions.</param>
    public WeatherResultsController(IRepository<WeatherResult> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all weather results stored in the system.
    /// </summary>
    /// <returns>A list of <see cref="WeatherResultResult"/> objects.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<WeatherResultResult>>> GetAll()
    {
        var results = await _repository.GetAllAsync();
        var mapped = _mapper.Map<IEnumerable<WeatherResultResult>>(results);
        return Ok(mapped);
    }

    /// <summary>
    /// Retrieves a specific weather result by its unique identifier.
    /// </summary>
    /// <param name="id">The unique ID of the result record.</param>
    /// <returns>The corresponding <see cref="WeatherResultResult"/> if found.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WeatherResultResult>> GetById(Guid id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result is null) return NotFound();

        var mapped = _mapper.Map<WeatherResultResult>(result);
        return Ok(mapped);
    }

    /// <summary>
    /// Creates a new weather result record.
    /// </summary>
    /// <param name="request">The <see cref="WeatherResultRequest"/> object containing result data.</param>
    /// <returns>The created <see cref="WeatherResultResult"/>.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WeatherResultResult>> Create([FromBody] WeatherResultRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = _mapper.Map<WeatherResult>(request);
        var created = await _repository.AddAsync(entity);
        var mapped = _mapper.Map<WeatherResultResult>(created);

        return CreatedAtAction(nameof(GetById), new { id = mapped.Id }, mapped);
    }

    /// <summary>
    /// Updates an existing weather result record.
    /// </summary>
    /// <param name="id">The ID of the record to update.</param>
    /// <param name="request">The updated result data.</param>
    /// <returns>The updated <see cref="WeatherResultResult"/>.</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WeatherResultResult>> Update(Guid id, [FromBody] WeatherResultRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return NotFound();

        _mapper.Map(request, existing);
        var updated = await _repository.UpdateAsync(existing);
        var mapped = _mapper.Map<WeatherResultResult>(updated);

        return Ok(mapped);
    }

    /// <summary>
    /// Deletes a specific weather result by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the result to delete.</param>
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
