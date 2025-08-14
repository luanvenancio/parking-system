using ParkingSystem.DTOs;
using ParkingSystem.Models;

namespace ParkingSystem.Services.Interfaces
{
    /// <summary>
    /// Service interface for parking spot operations
    /// </summary>
    public interface IParkingSpotService
    {
        /// <summary>
        /// Get all parking spots
        /// </summary>
        /// <returns>Collection of parking spot DTOs</returns>
        Task<IEnumerable<ParkingSpotDto>> GetAllParkingSpotsAsync();

        /// <summary>
        /// Get all parking spots for a specific parking lot
        /// </summary>
        /// <param name="parkingLotId">Parking lot id</param>
        /// <returns>Collection of parking spot DTOs</returns>
        Task<IEnumerable<ParkingSpotDto>> GetParkingSpotsByLotIdAsync(Guid parkingLotId);

        /// <summary>
        /// Get a specific parking spot by id
        /// </summary>
        /// <param name="id">Parking spot id</param>
        /// <returns>Parking spot DTO or null if not found</returns>
        Task<ParkingSpotDto?> GetParkingSpotByIdAsync(Guid id);

        /// <summary>
        /// Create a new parking spot
        /// </summary>
        /// <param name="parkingLotId">Parking lot id</param>
        /// <param name="spotTypeId">Spot type id</param>
        /// <param name="spotName">Spot name</param>
        /// <param name="floorLevel">Floor level</param>
        /// <returns>Created parking spot DTO</returns>
        Task<ParkingSpotDto> CreateParkingSpotAsync(Guid parkingLotId, Guid spotTypeId, string spotName, int floorLevel);

        /// <summary>
        /// Update parking spot status
        /// </summary>
        /// <param name="id">Parking spot id</param>
        /// <param name="status">New status</param>
        /// <returns>True if updated, false if not found</returns>
        Task<bool> UpdateParkingSpotStatusAsync(Guid id, SpotStatus status);

        /// <summary>
        /// Delete a parking spot
        /// </summary>
        /// <param name="id">Parking spot id</param>
        /// <returns>True if deleted, false if not found</returns>
        Task<bool> DeleteParkingSpotAsync(Guid id);
    }
}