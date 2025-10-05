using System.Linq.Expressions;

namespace WeatherLens.Data.Repositories
{
    /// <summary>
    /// Defines the base contract for all repository types, providing
    /// common CRUD operations for Entity Framework Core entities.
    /// </summary>
    /// <typeparam name="T">Entity type managed by this repository.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Retrieves all entities of type <typeparamref name="T"/>.
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Retrieves an entity by its unique identifier.
        /// </summary>
        Task<T?> GetByIdAsync(Guid id);

        /// <summary>
        /// Finds entities matching the specified condition.
        /// </summary>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Adds a new entity to the context and persists changes.
        /// </summary>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity and persists changes.
        /// </summary>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Removes an entity by its unique identifier.
        /// </summary>
        Task<bool> DeleteAsync(Guid id);
    }
}
