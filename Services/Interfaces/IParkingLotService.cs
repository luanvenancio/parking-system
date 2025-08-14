using ParkingSystem.DTOs;

namespace ParkingSystem.Services.Interfaces
{
    /// <summary>
    /// Service interface for parking lot operations
    /// </summary>
    public interface IParkingLotService
    {
        /// <summary>
        /// Get all parking lots
        /// </summary>
        /// <returns>Collection of parking lot DTOs</returns>
        Task<IEnumerable<ParkingLotDto>> GetAllParkingLotsAsync();

        /// <summary>
        /// Get a specific parking lot by id
        /// </summary>
        /// <param name="id">Parking lot id</param>
        /// <returns>Parking lot DTO or null if not found</returns>
        Task<ParkingLotDto?> GetParkingLotByIdAsync(Guid id);

        /// <summary>
        /// Create a new parking lot
        /// </summary>
        /// <param name="createDto">Parking lot creation data</param>
        /// <returns>Created parking lot DTO</returns>
        Task<ParkingLotDto> CreateParkingLotAsync(CreateParkingLotDto createDto);

        /// <summary>
        /// Update an existing parking lot
        /// </summary>
        /// <param name="id">Parking lot id</param>
        /// <param name="updateDto">Updated parking lot data</param>
        /// <returns>True if updated, false if not found</returns>
        Task<bool> UpdateParkingLotAsync(Guid id, CreateParkingLotDto updateDto);

        /// <summary>
        /// Delete a parking lot
        /// </summary>
        /// <param name="id">Parking lot id</param>
        /// <returns>True if deleted, false if not found</returns>
        Task<bool> DeleteParkingLotAsync(Guid id);
    }
}