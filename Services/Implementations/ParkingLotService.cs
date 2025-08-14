using ParkingSystem.DTOs;
using ParkingSystem.Models;
using ParkingSystem.Repositories.Interfaces;
using ParkingSystem.Services.Interfaces;

namespace ParkingSystem.Services.Implementations
{
    /// <summary>
    /// Implementation of the parking lot service
    /// </summary>
    public class ParkingLotService : IParkingLotService
    {
        private readonly IParkingLotRepository _parkingLotRepository;

        public ParkingLotService(IParkingLotRepository parkingLotRepository)
        {
            _parkingLotRepository = parkingLotRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ParkingLotDto>> GetAllParkingLotsAsync()
        {
            var parkingLots = await _parkingLotRepository.GetAllWithSpotCountAsync();

            return parkingLots.Select(pl => new ParkingLotDto
            {
                Id = pl.Id,
                Name = pl.Name,
                Address = pl.Address,
                TotalSpots = pl.ParkingSpots.Count
            });
        }

        /// <inheritdoc/>
        public async Task<ParkingLotDto?> GetParkingLotByIdAsync(Guid id)
        {
            var parkingLot = await _parkingLotRepository.GetParkingLotWithSpotsAsync(id);

            if (parkingLot == null)
            {
                return null;
            }

            return new ParkingLotDto
            {
                Id = parkingLot.Id,
                Name = parkingLot.Name,
                Address = parkingLot.Address,
                TotalSpots = parkingLot.ParkingSpots.Count
            };
        }

        /// <inheritdoc/>
        public async Task<ParkingLotDto> CreateParkingLotAsync(CreateParkingLotDto createDto)
        {
            var parkingLot = new ParkingLot
            {
                Name = createDto.Name,
                Address = createDto.Address,
                Description = createDto.Description
            };

            await _parkingLotRepository.AddAsync(parkingLot);
            await _parkingLotRepository.SaveChangesAsync();

            return new ParkingLotDto
            {
                Id = parkingLot.Id,
                Name = parkingLot.Name,
                Address = parkingLot.Address,
                TotalSpots = 0 // New parking lot has no spots yet
            };
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateParkingLotAsync(Guid id, CreateParkingLotDto updateDto)
        {
            var parkingLot = await _parkingLotRepository.GetByIdAsync(id);

            if (parkingLot == null)
            {
                return false;
            }

            parkingLot.Name = updateDto.Name;
            parkingLot.Address = updateDto.Address;
            parkingLot.Description = updateDto.Description;

            _parkingLotRepository.Update(parkingLot);
            return await _parkingLotRepository.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteParkingLotAsync(Guid id)
        {
            var parkingLot = await _parkingLotRepository.GetParkingLotWithSpotsAsync(id);

            if (parkingLot == null)
            {
                return false;
            }

            // Check if there are any parking spots with active sessions or reservations
            var hasActiveSessions = await _parkingLotRepository.HasActiveSessionsOrReservationsAsync(id);

            if (hasActiveSessions)
            {
                throw new InvalidOperationException("Cannot delete parking lot with active parking sessions or reservations");
            }

            _parkingLotRepository.Delete(parkingLot);
            return await _parkingLotRepository.SaveChangesAsync();
        }
    }
}