using Microsoft.EntityFrameworkCore;
using ParkingSystem.Data;
using ParkingSystem.Models;
using ParkingSystem.Repositories.Interfaces;


namespace ParkingSystem.Repositories.Implementations
{
    /// <summary>
    /// Repository implementation for parking lot operations
    /// </summary>
    public class ParkingLotRepository : Repository<ParkingLot>, IParkingLotRepository
    {
        public ParkingLotRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public async Task<ParkingLot?> GetParkingLotWithSpotsAsync(Guid id)
        {
            return await _context.ParkingLots
                .Include(pl => pl.ParkingSpots)
                    .ThenInclude(ps => ps.SpotType)
                .FirstOrDefaultAsync(pl => pl.Id == id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ParkingLot>> GetAllWithSpotCountAsync()
        {
            return await _context.ParkingLots
                .Include(pl => pl.ParkingSpots)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.ParkingLots.AnyAsync(pl => pl.Id == id);
        }

        /// <inheritdoc/>
        public async Task<bool> HasActiveSessionsOrReservationsAsync(Guid id)
        {
            var hasActiveSessions = await _context.ParkingSessions
                .AnyAsync(ps => ps.ParkingSpot!.ParkingLotId == id && ps.EndTime == null);

            if (hasActiveSessions)
            {
                return true;
            }

            var hasActiveReservations = await _context.Reservations
                .AnyAsync(r => r.ParkingSpot!.ParkingLotId == id && r.Status == ReservationStatus.Active);

            return hasActiveReservations;
        }
    }
}