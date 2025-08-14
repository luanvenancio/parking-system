namespace ParkingSystem.Repositories.Interfaces
{
    /// <summary>
    /// Generic repository interface for basic CRUD operations
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns>Collection of entities</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Get entity by id
        /// </summary>
        /// <param name="id">Entity id</param>
        /// <returns>Entity or null if not found</returns>
        Task<T?> GetByIdAsync(Guid id);

        /// <summary>
        /// Add a new entity
        /// </summary>
        /// <param name="entity">Entity to add</param>
        Task AddAsync(T entity);

        /// <summary>
        /// Update an existing entity
        /// </summary>
        /// <param name="entity">Entity to update</param>
        void Update(T entity);

        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        void Delete(T entity);

        /// <summary>
        /// Save changes to the database
        /// </summary>
        Task<bool> SaveChangesAsync();
    }
}