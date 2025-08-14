using Microsoft.EntityFrameworkCore;
using ParkingSystem.Data;
using ParkingSystem.Models;
using ParkingSystem.Repositories.Interfaces;


namespace ParkingSystem.Repositories.Implementations
{
    /// <summary>
    /// Repository implementation for reservation operations
    /// </summary>
    public class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        public ReservationRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public async Task<Reservation?> GetReservationWithDetailsAsync(Guid id)
        {
            return await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Car)
                .Include(r => r.ParkingSpot)
                    .ThenInclude(ps => ps!.SpotType)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Reservation>> GetAllWithDetailsAsync()
        {
            return await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Car)
                .Include(r => r.ParkingSpot)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Reservation>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Reservations
                .Include(r => r.Car)
                .Include(r => r.ParkingSpot)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Reservation>> GetByParkingSpotIdAsync(Guid parkingSpotId)
        {
            return await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Car)
                .Where(r => r.ParkingSpotId == parkingSpotId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<bool> HasOverlappingReservationsAsync(Guid parkingSpotId, DateTime startTime, DateTime endTime, Guid? excludeReservationId = null)
        {
            var query = _context.Reservations
                .Where(r => r.ParkingSpotId == parkingSpotId && r.Status == ReservationStatus.Active);

            if (excludeReservationId.HasValue)
            {
                query = query.Where(r => r.Id != excludeReservationId.Value);
            }
            return await query.AnyAsync(r =>
                r.StartTime < endTime && r.EndTime > startTime);
        }
    }
}