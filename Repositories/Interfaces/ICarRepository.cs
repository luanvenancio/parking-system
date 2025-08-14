using ParkingSystem.Models;

namespace ParkingSystem.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for car operations
    /// </summary>
    public interface ICarRepository : IRepository<Car>
    {
        /// <summary>
        /// Check if a car belongs to a specific user
        /// </summary>
        /// <param name="carId">Car id</param>
        /// <param name="userId">User id</param>
        /// <returns>True if the car belongs to the user, false otherwise</returns>
        Task<bool> IsCarOwnedByUserAsync(Guid carId, Guid userId);
    }
}