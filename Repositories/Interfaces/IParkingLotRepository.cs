using ParkingSystem.Models;

namespace ParkingSystem.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for parking lot operations
    /// </summary>
    public interface IParkingLotRepository : IRepository<ParkingLot>
    {
        /// <summary>
        /// Get parking lot with all related parking spots
        /// </summary>
        /// <param name="id">Parking lot id</param>
        /// <returns>Parking lot with spots or null if not found</returns>
        Task<ParkingLot?> GetParkingLotWithSpotsAsync(Guid id);

        /// <summary>
        /// Get all parking lots with their spots count
        /// </summary>
        /// <returns>Collection of parking lots with spots count</returns>
        Task<IEnumerable<ParkingLot>> GetAllWithSpotCountAsync();

        /// <summary>
        /// Check if a parking lot exists
        /// </summary>
        /// <param name="id">Parking lot id</param>
        /// <returns>True if exists, false otherwise</returns>
        Task<bool> ExistsAsync(Guid id);

        /// <summary>
        /// Check if a parking lot has active sessions or reservations
        /// </summary>
        /// <param name="id">Parking lot id</param>
        /// <returns>True if has active sessions or reservations, false otherwise</returns>
        Task<bool> HasActiveSessionsOrReservationsAsync(Guid id);
    }
}