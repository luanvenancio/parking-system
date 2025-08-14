using Microsoft.EntityFrameworkCore;
using ParkingSystem.Data;
using ParkingSystem.Models;
using ParkingSystem.Repositories.Interfaces;


namespace ParkingSystem.Repositories.Implementations
{
    /// <summary>
    /// Repository implementation for parking spot operations
    /// </summary>
    public class ParkingSpotRepository : Repository<ParkingSpot>, IParkingSpotRepository
    {
        public ParkingSpotRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ParkingSpot>> GetByParkingLotIdAsync(Guid parkingLotId)
        {
            return await _context.ParkingSpots
                .Include(ps => ps.SpotType)
                .Where(ps => ps.ParkingLotId == parkingLotId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ParkingSpot?> GetWithTypeAsync(Guid id)
        {
            return await _context.ParkingSpots
                .Include(ps => ps.SpotType)
                .FirstOrDefaultAsync(ps => ps.Id == id);
        }

        /// <inheritdoc/>
        public async Task<bool> SpotNameExistsInLotAsync(Guid parkingLotId, string spotName)
        {
            return await _context.ParkingSpots
                .AnyAsync(ps => ps.ParkingLotId == parkingLotId && ps.SpotName == spotName);
        }

        /// <inheritdoc/>
        public async Task<bool> HasActiveSessionsAsync(Guid id)
        {
            return await _context.ParkingSessions
                .AnyAsync(ps => ps.ParkingSpotId == id && ps.EndTime == null);
        }

        /// <inheritdoc/>
        public async Task<bool> HasActiveReservationsAsync(Guid id)
        {
            return await _context.Reservations
                .AnyAsync(r => r.ParkingSpotId == id && r.Status == ReservationStatus.Active);
        }
    }
}