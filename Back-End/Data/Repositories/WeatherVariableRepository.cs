using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WeatherLens.Models;

namespace WeatherLens.Data.Repositories;

/// <summary>
/// Repository for managing <see cref="WeatherVariable"/> entities.
/// </summary>
public sealed class WeatherVariableRepository : IRepository<WeatherVariable>
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Initializes a new instance of <see cref="WeatherVariableRepository"/> using the provided EF Core context.
    /// </summary>
    public WeatherVariableRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<WeatherVariable>> GetAllAsync()
    {
        return await _context.Variables.AsNoTracking().ToListAsync();
    }

    /// <inheritdoc />
    public async Task<WeatherVariable?> GetByIdAsync(Guid id)
    {
        return await _context.Variables.FindAsync(id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<WeatherVariable>> FindAsync(Expression<Func<WeatherVariable, bool>> predicate)
    {
        return await _context.Variables.AsNoTracking().Where(predicate).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<WeatherVariable> AddAsync(WeatherVariable entity)
    {
        await _context.Variables.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <inheritdoc />
    public async Task<WeatherVariable> UpdateAsync(WeatherVariable entity)
    {
        _context.Variables.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(Guid id)
    {
        var variable = await _context.Variables.FindAsync(id);

        if (variable == null) return false;

        _context.Variables.Remove(variable);
        await _context.SaveChangesAsync();
        return true;
    }
}
