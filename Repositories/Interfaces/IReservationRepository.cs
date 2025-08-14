using ParkingSystem.Models;


namespace ParkingSystem.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for reservation operations
    /// </summary>
    public interface IReservationRepository : IRepository<Reservation>
    {
        /// <summary>
        /// Get reservation with all related entities (User, Car, ParkingSpot)
        /// </summary>
        /// <param name="id">Reservation id</param>
        /// <returns>Reservation with related entities or null if not found</returns>
        Task<Reservation?> GetReservationWithDetailsAsync(Guid id);

        /// <summary>
        /// Get all reservations with related entities
        /// </summary>
        /// <returns>Collection of reservations with related entities</returns>
        Task<IEnumerable<Reservation>> GetAllWithDetailsAsync();

        /// <summary>
        /// Get all reservations for a specific user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Collection of reservations</returns>
        Task<IEnumerable<Reservation>> GetByUserIdAsync(Guid userId);

        /// <summary>
        /// Get all reservations for a specific parking spot
        /// </summary>
        /// <param name="parkingSpotId">Parking spot id</param>
        /// <returns>Collection of reservations</returns>
        Task<IEnumerable<Reservation>> GetByParkingSpotIdAsync(Guid parkingSpotId);

        /// <summary>
        /// Check if there are any overlapping reservations for a parking spot
        /// </summary>
        /// <param name="parkingSpotId">Parking spot id</param>
        /// <param name="startTime">Start time</param>
        /// <param name="endTime">End time</param>
        /// <param name="excludeReservationId">Optional reservation id to exclude from check (for updates)</param>
        /// <returns>True if there are overlapping reservations, false otherwise</returns>
        Task<bool> HasOverlappingReservationsAsync(Guid parkingSpotId, DateTime startTime, DateTime endTime, Guid? excludeReservationId = null);
    }
}