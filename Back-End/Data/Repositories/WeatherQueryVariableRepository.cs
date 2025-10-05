using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WeatherLens.Models;

namespace WeatherLens.Data.Repositories;

/// <summary>
/// Repository for managing <see cref="WeatherQueryVariable"/> entities.
/// </summary>
public sealed class WeatherQueryVariableRepository : IRepository<WeatherQueryVariable>
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Initializes a new instance of <see cref="WeatherQueryVariableRepository"/> using the provided EF Core context.
    /// </summary>
    public WeatherQueryVariableRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<WeatherQueryVariable>> GetAllAsync()
    {
        return await _context.QueryVariables
            .Include(qv => qv.Query)
            .Include(qv => qv.Variable)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<WeatherQueryVariable?> GetByIdAsync(Guid id)
    {
        return await _context.QueryVariables
            .Include(qv => qv.Query)
            .Include(qv => qv.Variable)
            .FirstOrDefaultAsync(qv => qv.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<WeatherQueryVariable>> FindAsync(Expression<Func<WeatherQueryVariable, bool>> predicate)
    {
        return await _context.QueryVariables
            .Include(qv => qv.Query)
            .Include(qv => qv.Variable)
            .AsNoTracking()
            .Where(predicate)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<WeatherQueryVariable> AddAsync(WeatherQueryVariable entity)
    {
        await _context.QueryVariables.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <inheritdoc />
    public async Task<WeatherQueryVariable> UpdateAsync(WeatherQueryVariable entity)
    {
        _context.QueryVariables.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(Guid id)
    {
        var qv = await _context.QueryVariables.FindAsync(id);

        if (qv == null) return false;

        _context.QueryVariables.Remove(qv);
        await _context.SaveChangesAsync();
        return true;
    }
}
