using Microsoft.AspNetCore.Mvc;
using WeatherLens.Data.Repositories;
using WeatherLens.Models;
using WeatherLens.DTOs;
using AutoMapper;

namespace WeatherLens.Controllers;

/// <summary>
/// Provides endpoints for managing application users.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class UsersController : ControllerBase
{
    private readonly IRepository<User> _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="repository">Repository for user persistence.</param>
    /// <param name="mapper">AutoMapper instance for DTO conversions.</param>
    public UsersController(IRepository<User> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all registered users.
    /// </summary>
    /// <returns>A list of <see cref="UserResult"/> objects.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserResult>>> GetAll()
    {
        var users = await _repository.GetAllAsync();
        var results = _mapper.Map<IEnumerable<UserResult>>(users);
        return Ok(results);
    }

    /// <summary>
    /// Retrieves a specific user by unique identifier.
    /// </summary>
    /// <param name="id">The unique ID of the user.</param>
    /// <returns>The corresponding <see cref="UserResult"/> if found.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResult>> GetById(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user is null) return NotFound();

        var result = _mapper.Map<UserResult>(user);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new user record.
    /// </summary>
    /// <param name="request">The <see cref="UserRequest"/> object containing user details.</param>
    /// <returns>The created <see cref="UserResult"/>.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResult>> Create([FromBody] UserRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var entity = _mapper.Map<User>(request);
        var created = await _repository.AddAsync(entity);
        var result = _mapper.Map<UserResult>(created);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Updates an existing user record.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="request">The updated user data.</param>
    /// <returns>The updated <see cref="UserResult"/>.</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResult>> Update(Guid id, [FromBody] UserRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return NotFound();

        _mapper.Map(request, existing); // apply updates
        var updated = await _repository.UpdateAsync(existing);
        var result = _mapper.Map<UserResult>(updated);

        return Ok(result);
    }

    /// <summary>
    /// Deletes a user by unique identifier.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
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
