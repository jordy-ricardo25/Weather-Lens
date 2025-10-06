using Microsoft.AspNetCore.Mvc;
using WeatherLens.Data.Repositories;
using WeatherLens.Models;
using WeatherLens.DTOs;
using AutoMapper;

namespace WeatherLens.Controllers;

/// <summary>
/// Provides endpoints for managing and retrieving weather variable metadata.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class WeatherVariablesController : ControllerBase
{
    private readonly IRepository<WeatherVariable> _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="WeatherVariablesController"/> class.
    /// </summary>
    /// <param name="repository">Generic repository for managing weather variables.</param>
    /// <param name="mapper">AutoMapper instance for mapping DTOs and entities.</param>
    public WeatherVariablesController(IRepository<WeatherVariable> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all weather variables defined in the system.
    /// </summary>
    /// <returns>A list of <see cref="WeatherVariableResult"/> objects.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<WeatherVariableResult>>> GetAll()
    {
        var variables = await _repository.GetAllAsync();
        var results = _mapper.Map<IEnumerable<WeatherVariableResult>>(variables);
        return Ok(results);
    }

    /// <summary>
    /// Retrieves a specific weather variable by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the variable.</param>
    /// <returns>The corresponding <see cref="WeatherVariableResult"/> if found.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WeatherVariableResult>> GetById(Guid id)
    {
        var variable = await _repository.GetByIdAsync(id);
        if (variable is null) return NotFound();

        var result = _mapper.Map<WeatherVariableResult>(variable);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new weather variable entry.
    /// </summary>
    /// <param name="request">The <see cref="WeatherVariableRequest"/> object containing the variable details.</param>
    /// <returns>The created <see cref="WeatherVariableResult"/>.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WeatherVariableResult>> Create([FromBody] WeatherVariableRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = _mapper.Map<WeatherVariable>(request);
        var created = await _repository.AddAsync(entity);
        var result = _mapper.Map<WeatherVariableResult>(created);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Updates an existing weather variable entry.
    /// </summary>
    /// <param name="id">The ID of the variable to update.</param>
    /// <param name="request">The updated variable data.</param>
    /// <returns>The updated <see cref="WeatherVariableResult"/>.</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WeatherVariableResult>> Update(Guid id, [FromBody] WeatherVariableRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return NotFound();

        _mapper.Map(request, existing);
        var updated = await _repository.UpdateAsync(existing);
        var result = _mapper.Map<WeatherVariableResult>(updated);

        return Ok(result);
    }

    /// <summary>
    /// Deletes a weather variable by unique identifier.
    /// </summary>
    /// <param name="id">The ID of the variable to delete.</param>
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
