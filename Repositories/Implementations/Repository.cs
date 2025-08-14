using Microsoft.EntityFrameworkCore;
using ParkingSystem.Data;
using ParkingSystem.Repositories.Interfaces;


namespace ParkingSystem.Repositories.Implementations
{
    /// <summary>
    /// Generic repository implementation for basic CRUD operations
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <inheritdoc/>
        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <inheritdoc/>
        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        /// <inheritdoc/>
        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        /// <inheritdoc/>
        public virtual void Delete(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        /// <inheritdoc/>
        public virtual async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}