using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WeatherLens.Models;

namespace WeatherLens.Data.Repositories;

/// <summary>
/// Repository for managing <see cref="User"/> entities.
/// </summary>
public sealed class UserRepository : IRepository<User>
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Initializes a new instance of <see cref="UserRepository"/> using the provided EF Core context.
    /// </summary>
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.AsNoTracking().ToListAsync();
    }

    /// <inheritdoc />
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate)
    {
        return await _context.Users.AsNoTracking().Where(predicate).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<User> AddAsync(User entity)
    {
        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <inheritdoc />
    public async Task<User> UpdateAsync(User entity)
    {
        _context.Users.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
