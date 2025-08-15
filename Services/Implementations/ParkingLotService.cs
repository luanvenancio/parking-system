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

        private ParkingLotDetailDto MapToDetailDto(ParkingLot parkingLot)
        {
            var totalSpots = parkingLot.ParkingSpots.Count;
            var availableSpots = parkingLot.ParkingSpots.Count(ps => ps.Status == SpotStatus.Available);
            var occupiedSpots = totalSpots - availableSpots;

            var spotTypesDetails = parkingLot.ParkingSpots
                .Where(ps => ps.SpotType != null)
                .GroupBy(ps => ps.SpotType)
               .Select(g =>
        {
            var total = g.Count();
            var available = g.Count(s => s.Status == SpotStatus.Available);
            var hourlyRate = g.Key.Fee?.FeeRules?.FirstOrDefault(r => r.ChargeType == ChargeType.Hourly)?.ChargeAmount;

            return new SpotTypeDetailDto
            {
                TypeName = g.Key.Name,
                Description = g.Key.Description,
                TotalCount = total,
                AvailableCount = available,
                OccupiedCount = total - available,
                DailyMaxCap = g.Key.Fee?.DailyMaxCap,
                HourlyRate = hourlyRate,
                PricingDetails = hourlyRate.HasValue
                    ? $"Taxa por Hora: {hourlyRate:C2}. Máximo Diário: {g.Key.Fee?.DailyMaxCap:C2}."
                    : "Preço não configurado."
            };
        })
        .ToList();

            var detailedSpots = parkingLot.ParkingSpots
                .Select(ps => new ParkingSpotSummaryDto
                {
                    Id = ps.Id,
                    SpotName = ps.SpotName,
                    FloorLevel = ps.FloorLevel,
                    Status = ps.Status.ToString(),
                    TypeName = ps.SpotType?.Name ?? "N/A"

                })
                 .OrderBy(s => s.FloorLevel).ThenBy(s => s.SpotName)
                 .ToList();

            return new ParkingLotDetailDto
            {
                Id = parkingLot.Id,
                Name = parkingLot.Name,
                Address = parkingLot.Address,
                Description = parkingLot.Description,
                OperatingHours = parkingLot.OperatingHours,
                Latitude = parkingLot.Latitude,
                Longitude = parkingLot.Longitude,
                TotalSpots = totalSpots,
                AvailableSpots = availableSpots,
                OccupiedSpots = occupiedSpots,
                OccupancyPercentage = totalSpots > 0 ? Math.Round((double)occupiedSpots / totalSpots * 100, 2) : 0,
                SpotTypesDetails = spotTypesDetails,
                Spots = detailedSpots
            };
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ParkingLotDetailDto>> GetAllParkingLotsAsync()
        {
            var parkingLots = await _parkingLotRepository.GetAllWithSpotCountAsync();

            return parkingLots.Select(MapToDetailDto);
        }

        /// <inheritdoc/>
        public async Task<ParkingLotDetailDto?> GetParkingLotByIdAsync(Guid id)
        {
            var parkingLot = await _parkingLotRepository.GetParkingLotWithSpotsAsync(id);

            if (parkingLot == null)
            {
                return null;
            }

            return MapToDetailDto(parkingLot);
        }

        /// <inheritdoc/>
        public async Task<ParkingLotDto> CreateParkingLotAsync(CreateParkingLotDto createDto)
        {
            var parkingLot = new ParkingLot
            {
                Name = createDto.Name,
                Address = createDto.Address,
                Description = createDto.Description,
                OperatingHours = createDto.OperatingHours,
                Latitude = createDto.Latitude,
                Longitude = createDto.Longitude
            };


            await _parkingLotRepository.AddAsync(parkingLot);
            await _parkingLotRepository.SaveChangesAsync();

            return new ParkingLotDto
            {
                Id = parkingLot.Id,
                Name = parkingLot.Name,
                Address = parkingLot.Address,
                TotalSpots = 0
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
            parkingLot.OperatingHours = updateDto.OperatingHours;
            parkingLot.Latitude = updateDto.Latitude;
            parkingLot.Longitude = updateDto.Longitude;

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