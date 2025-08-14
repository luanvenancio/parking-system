using Microsoft.EntityFrameworkCore;
using ParkingSystem.Data;
using ParkingSystem.Models;
using ParkingSystem.Repositories.Interfaces;

namespace ParkingSystem.Repositories.Implementations
{
    /// <summary>
    /// Repository implementation for car operations
    /// </summary>
    public class CarRepository : Repository<Car>, ICarRepository
    {
        public CarRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public async Task<bool> IsCarOwnedByUserAsync(Guid carId, Guid userId)
        {
            return await _context.Cars
                .Include(c => c.Users)
                .AnyAsync(c => c.Id == carId && c.Users.Any(u => u.Id == userId));
        }
    }
}