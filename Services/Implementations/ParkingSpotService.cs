using System;
using ParkingSystem.DTOs;
using ParkingSystem.Models;
using ParkingSystem.Repositories.Interfaces;
using ParkingSystem.Services.Interfaces;


namespace ParkingSystem.Services.Implementations
{
    /// <summary>
    /// Implementation of the parking spot service
    /// </summary>
    public class ParkingSpotService : IParkingSpotService
    {
        private readonly IParkingSpotRepository _parkingSpotRepository;
        private readonly IParkingLotRepository _parkingLotRepository;
        private readonly IRepository<SpotType> _spotTypeRepository;

        public ParkingSpotService(
            IParkingSpotRepository parkingSpotRepository,
            IParkingLotRepository parkingLotRepository,
            IRepository<SpotType> spotTypeRepository)
        {
            _parkingSpotRepository = parkingSpotRepository;
            _parkingLotRepository = parkingLotRepository;
            _spotTypeRepository = spotTypeRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ParkingSpotDto>> GetAllParkingSpotsAsync()
        {
            var parkingSpots = await _parkingSpotRepository.GetAllAsync();
            var result = new List<ParkingSpotDto>();

            foreach (var spot in parkingSpots)
            {
                var spotWithType = await _parkingSpotRepository.GetWithTypeAsync(spot.Id);
                if (spotWithType != null && spotWithType.SpotType != null)
                {
                    result.Add(new ParkingSpotDto
                    {
                        Id = spotWithType.Id,
                        SpotName = spotWithType.SpotName,
                        FloorLevel = spotWithType.FloorLevel,
                        Status = spotWithType.Status.ToString(),
                        SpotTypeName = spotWithType.SpotType.Name
                    });
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ParkingSpotDto>> GetParkingSpotsByLotIdAsync(Guid parkingLotId)
        {
            var parkingSpots = await _parkingSpotRepository.GetByParkingLotIdAsync(parkingLotId);

            return parkingSpots.Select(ps => new ParkingSpotDto
            {
                Id = ps.Id,
                SpotName = ps.SpotName,
                FloorLevel = ps.FloorLevel,
                Status = ps.Status.ToString(),
                SpotTypeName = ps.SpotType?.Name ?? "Unknown"
            });
        }

        /// <inheritdoc/>
        public async Task<ParkingSpotDto?> GetParkingSpotByIdAsync(Guid id)
        {
            var parkingSpot = await _parkingSpotRepository.GetWithTypeAsync(id);

            if (parkingSpot == null)
            {
                return null;
            }

            return new ParkingSpotDto
            {
                Id = parkingSpot.Id,
                SpotName = parkingSpot.SpotName,
                FloorLevel = parkingSpot.FloorLevel,
                Status = parkingSpot.Status.ToString(),
                SpotTypeName = parkingSpot.SpotType?.Name ?? "Unknown"
            };
        }

        /// <inheritdoc/>
        public async Task<ParkingSpotDto> CreateParkingSpotAsync(Guid parkingLotId, Guid spotTypeId, string spotName, int floorLevel)
        {
            // Verify parking lot exists
            var parkingLotExists = await _parkingLotRepository.ExistsAsync(parkingLotId);
            if (!parkingLotExists)
            {
                throw new ArgumentException($"Parking lot with ID {parkingLotId} not found");
            }

            // Verify spot type exists
            var spotType = await _spotTypeRepository.GetByIdAsync(spotTypeId);
            if (spotType == null)
            {
                throw new ArgumentException($"Spot type with ID {spotTypeId} not found");
            }

            // Check if spot name is already used in this parking lot
            var spotExists = await _parkingSpotRepository.SpotNameExistsInLotAsync(parkingLotId, spotName);

            if (spotExists)
            {
                throw new InvalidOperationException($"Spot name '{spotName}' already exists in this parking lot");
            }

            var parkingSpot = new ParkingSpot
            {
                SpotName = spotName,
                FloorLevel = floorLevel,
                Status = SpotStatus.Available,
                ParkingLotId = parkingLotId,
                SpotTypeId = spotTypeId
            };

            await _parkingSpotRepository.AddAsync(parkingSpot);
            await _parkingSpotRepository.SaveChangesAsync();

            return new ParkingSpotDto
            {
                Id = parkingSpot.Id,
                SpotName = parkingSpot.SpotName,
                FloorLevel = parkingSpot.FloorLevel,
                Status = parkingSpot.Status.ToString(),
                SpotTypeName = spotType.Name
            };
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateParkingSpotStatusAsync(Guid id, SpotStatus status)
        {
            var parkingSpot = await _parkingSpotRepository.GetByIdAsync(id);

            if (parkingSpot == null)
            {
                return false;
            }

            // Check if there are active sessions when trying to set to Available
            if (status == SpotStatus.Available)
            {
                var hasActiveSessions = await _parkingSpotRepository.HasActiveSessionsAsync(id);

                if (hasActiveSessions)
                {
                    throw new InvalidOperationException("Cannot set spot to Available when there are active parking sessions");
                }
            }

            parkingSpot.Status = status;
            _parkingSpotRepository.Update(parkingSpot);
            return await _parkingSpotRepository.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteParkingSpotAsync(Guid id)
        {
            var parkingSpot = await _parkingSpotRepository.GetByIdAsync(id);

            if (parkingSpot == null)
            {
                return false;
            }

            // Check if there are any active sessions for this spot
            var hasActiveSessions = await _parkingSpotRepository.HasActiveSessionsAsync(id);

            if (hasActiveSessions)
            {
                throw new InvalidOperationException("Cannot delete parking spot with active parking sessions");
            }

            // Check if there are any active reservations for this spot
            var hasActiveReservations = await _parkingSpotRepository.HasActiveReservationsAsync(id);

            if (hasActiveReservations)
            {
                throw new InvalidOperationException("Cannot delete parking spot with active reservations");
            }

            _parkingSpotRepository.Delete(parkingSpot);
            return await _parkingSpotRepository.SaveChangesAsync();
        }
    }
}