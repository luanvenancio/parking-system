using ParkingSystem.Models;

namespace ParkingSystem.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for parking spot operations
    /// </summary>
    public interface IParkingSpotRepository : IRepository<ParkingSpot>
    {
        /// <summary>
        /// Get all parking spots for a specific parking lot
        /// </summary>
        /// <param name="parkingLotId">Parking lot id</param>
        /// <returns>Collection of parking spots</returns>
        Task<IEnumerable<ParkingSpot>> GetByParkingLotIdAsync(Guid parkingLotId);

        /// <summary>
        /// Get parking spot with its type information
        /// </summary>
        /// <param name="id">Parking spot id</param>
        /// <returns>Parking spot with type or null if not found</returns>
        Task<ParkingSpot?> GetWithTypeAsync(Guid id);

        /// <summary>
        /// Check if a spot name already exists in a parking lot
        /// </summary>
        /// <param name="parkingLotId">Parking lot id</param>
        /// <param name="spotName">Spot name</param>
        /// <returns>True if exists, false otherwise</returns>
        Task<bool> SpotNameExistsInLotAsync(Guid parkingLotId, string spotName);

        /// <summary>
        /// Check if a parking spot has active sessions
        /// </summary>
        /// <param name="id">Parking spot id</param>
        /// <returns>True if has active sessions, false otherwise</returns>
        Task<bool> HasActiveSessionsAsync(Guid id);

        /// <summary>
        /// Check if a parking spot has active reservations
        /// </summary>
        /// <param name="id">Parking spot id</param>
        /// <returns>True if has active reservations, false otherwise</returns>
        Task<bool> HasActiveReservationsAsync(Guid id);
    }
}