using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WeatherLens.Models;

namespace WeatherLens.Data.Repositories;

/// <summary>
/// Repository for managing <see cref="WeatherResult"/> entities.
/// </summary>
public sealed class WeatherResultRepository : IRepository<WeatherResult>
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Initializes a new instance of <see cref="WeatherResultRepository"/> using the provided EF Core context.
    /// </summary>
    public WeatherResultRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<WeatherResult>> GetAllAsync()
    {
        return await _context.Results
            .Include(r => r.Query)
            .Include(r => r.Variable)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<WeatherResult?> GetByIdAsync(Guid id)
    {
        return await _context.Results
            .Include(r => r.Query)
            .Include(r => r.Variable)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<WeatherResult>> FindAsync(Expression<Func<WeatherResult, bool>> predicate)
    {
        return await _context.Results
            .Include(r => r.Query)
            .Include(r => r.Variable)
            .AsNoTracking()
            .Where(predicate)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<WeatherResult> AddAsync(WeatherResult entity)
    {
        await _context.Results.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <inheritdoc />
    public async Task<WeatherResult> UpdateAsync(WeatherResult entity)
    {
        _context.Results.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await _context.Results.FindAsync(id);

        if (result == null) return false;

        _context.Results.Remove(result);
        await _context.SaveChangesAsync();
        return true;
    }
}
