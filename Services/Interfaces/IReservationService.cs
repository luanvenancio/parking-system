using ParkingSystem.DTOs;

namespace ParkingSystem.Services.Interfaces
{
    /// <summary>
    /// Service interface for reservation operations
    /// </summary>
    public interface IReservationService
    {
        /// <summary>
        /// Get all reservations
        /// </summary>
        /// <returns>Collection of reservation DTOs</returns>
        Task<IEnumerable<ReservationDto>> GetAllReservationsAsync();

        /// <summary>
        /// Get a specific reservation by id
        /// </summary>
        /// <param name="id">Reservation id</param>
        /// <returns>Reservation DTO or null if not found</returns>
        Task<ReservationDto?> GetReservationByIdAsync(Guid id);

        /// <summary>
        /// Get all reservations for a specific user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Collection of reservation DTOs</returns>
        Task<IEnumerable<ReservationDto>> GetReservationsByUserIdAsync(Guid userId);

        /// <summary>
        /// Get all reservations for a specific parking spot
        /// </summary>
        /// <param name="parkingSpotId">Parking spot id</param>
        /// <returns>Collection of reservation DTOs</returns>
        Task<IEnumerable<ReservationDto>> GetReservationsByParkingSpotIdAsync(Guid parkingSpotId);

        /// <summary>
        /// Create a new reservation
        /// </summary>
        /// <param name="createDto">Reservation creation data</param>
        /// <returns>Created reservation DTO</returns>
        Task<ReservationDto> CreateReservationAsync(CreateReservationDto createDto);

        /// <summary>
        /// Update reservation status
        /// </summary>
        /// <param name="id">Reservation id</param>
        /// <param name="updateDto">Updated status data</param>
        /// <returns>True if updated, false if not found</returns>
        Task<bool> UpdateReservationStatusAsync(Guid id, UpdateReservationStatusDto updateDto);

        /// <summary>
        /// Cancel a reservation
        /// </summary>
        /// <param name="id">Reservation id</param>
        /// <returns>True if cancelled, false if not found</returns>
        Task<bool> CancelReservationAsync(Guid id);
    }
}