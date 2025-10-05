using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WeatherLens.Models;

namespace WeatherLens.Data.Repositories;

/// <summary>
/// Repository for managing <see cref="Location"/> entities.
/// </summary>
public sealed class LocationRepository : IRepository<Location>
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Initializes a new instance of <see cref="LocationRepository"/> using the provided EF Core context.
    /// </summary>
    public LocationRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Location>> GetAllAsync()
    {
        return await _context.Locations.AsNoTracking().ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Location?> GetByIdAsync(Guid id)
    {
        return await _context.Locations.FindAsync(id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Location>> FindAsync(Expression<Func<Location, bool>> predicate)
    {
        return await _context.Locations.AsNoTracking().Where(predicate).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Location> AddAsync(Location entity)
    {
        await _context.Locations.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <inheritdoc />
    public async Task<Location> UpdateAsync(Location entity)
    {
        _context.Locations.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(Guid id)
    {
        var loc = await _context.Locations.FindAsync(id);
        if (loc == null) return false;
        _context.Locations.Remove(loc);
        await _context.SaveChangesAsync();
        return true;
    }
}
