using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WeatherLens.Models;

namespace WeatherLens.Data.Repositories;

/// <summary>
/// Repository for managing <see cref="WeatherQuery"/> entities.
/// </summary>
public sealed class WeatherQueryRepository : IRepository<WeatherQuery>
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Initializes a new instance of <see cref="WeatherQueryRepository"/> using the provided EF Core context.
    /// </summary>
    public WeatherQueryRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<WeatherQuery>> GetAllAsync()
    {
        return await _context.Queries
            .Include(q => q.User)
            .Include(q => q.Location)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<WeatherQuery?> GetByIdAsync(Guid id)
    {
        return await _context.Queries
            .Include(q => q.User)
            .Include(q => q.Location)
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<WeatherQuery>> FindAsync(Expression<Func<WeatherQuery, bool>> predicate)
    {
        return await _context.Queries
            .Include(q => q.User)
            .Include(q => q.Location)
            .AsNoTracking()
            .Where(predicate)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<WeatherQuery> AddAsync(WeatherQuery entity)
    {
        await _context.Queries.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <inheritdoc />
    public async Task<WeatherQuery> UpdateAsync(WeatherQuery entity)
    {
        _context.Queries.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(Guid id)
    {
        var query = await _context.Queries.FindAsync(id);

        if (query == null) return false;

        _context.Queries.Remove(query);
        await _context.SaveChangesAsync();
        return true;
    }
}
