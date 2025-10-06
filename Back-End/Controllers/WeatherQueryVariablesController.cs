using Microsoft.AspNetCore.Mvc;
using WeatherLens.Data.Repositories;
using WeatherLens.Models;
using WeatherLens.DTOs;
using AutoMapper;

namespace WeatherLens.Controllers;

/// <summary>
/// Provides endpoints for mapping weather queries and their associated variables.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class WeatherQueryVariablesController : ControllerBase
{
    private readonly IRepository<WeatherQueryVariable> _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="WeatherQueryVariablesController"/> class.
    /// </summary>
    /// <param name="repository">Repository used to manage query-variable relationships.</param>
    /// <param name="mapper">AutoMapper instance for DTO conversions.</param>
    public WeatherQueryVariablesController(IRepository<WeatherQueryVariable> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all query-variable relationships from the database.
    /// </summary>
    /// <returns>A collection of <see cref="WeatherQueryVariableResult"/> objects.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<WeatherQueryVariableResult>>> GetAll()
    {
        var records = await _repository.GetAllAsync();
        var results = _mapper.Map<IEnumerable<WeatherQueryVariableResult>>(records);
        return Ok(results);
    }

    /// <summary>
    /// Retrieves a specific query-variable relationship by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the record.</param>
    /// <returns>The corresponding <see cref="WeatherQueryVariableResult"/> if found.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WeatherQueryVariableResult>> GetById(Guid id)
    {
        var record = await _repository.GetByIdAsync(id);
        if (record is null) return NotFound();

        var result = _mapper.Map<WeatherQueryVariableResult>(record);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new link between a weather query and a variable.
    /// </summary>
    /// <param name="request">The <see cref="WeatherQueryVariableRequest"/> object containing query and variable IDs.</param>
    /// <returns>The created <see cref="WeatherQueryVariableResult"/>.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WeatherQueryVariableResult>> Create([FromBody] WeatherQueryVariableRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = _mapper.Map<WeatherQueryVariable>(request);
        var created = await _repository.AddAsync(entity);
        var result = _mapper.Map<WeatherQueryVariableResult>(created);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Updates an existing query-variable relationship.
    /// </summary>
    /// <param name="id">The ID of the record to update.</param>
    /// <param name="request">The updated relationship data.</param>
    /// <returns>The updated <see cref="WeatherQueryVariableResult"/>.</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WeatherQueryVariableResult>> Update(Guid id, [FromBody] WeatherQueryVariableRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return NotFound();

        _mapper.Map(request, existing);
        var updated = await _repository.UpdateAsync(existing);
        var result = _mapper.Map<WeatherQueryVariableResult>(updated);

        return Ok(result);
    }

    /// <summary>
    /// Deletes a specific query-variable relationship by unique identifier.
    /// </summary>
    /// <param name="id">The ID of the record to delete.</param>
    /// <returns>A 204 No Content response if the deletion succeeds.</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _repository.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
