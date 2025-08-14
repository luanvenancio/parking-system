using ParkingSystem.DTOs;
using ParkingSystem.Models;
using ParkingSystem.Repositories.Interfaces;
using ParkingSystem.Services.Interfaces;


namespace ParkingSystem.Services.Implementations
{
    /// <summary>
    /// Service implementation for reservation operations
    /// </summary>
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IParkingSpotRepository _parkingSpotRepository;
        private readonly IRepository<User> _userRepository;
        private readonly ICarRepository _carRepository;

        public ReservationService(
            IReservationRepository reservationRepository,
            IParkingSpotRepository parkingSpotRepository,
            IRepository<User> userRepository,
            ICarRepository carRepository)
        {
            _reservationRepository = reservationRepository;
            _parkingSpotRepository = parkingSpotRepository;
            _userRepository = userRepository;
            _carRepository = carRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ReservationDto>> GetAllReservationsAsync()
        {
            var reservations = await _reservationRepository.GetAllWithDetailsAsync();
            return reservations.Select(MapToDto);
        }

        /// <inheritdoc/>
        public async Task<ReservationDto?> GetReservationByIdAsync(Guid id)
        {
            var reservation = await _reservationRepository.GetReservationWithDetailsAsync(id);
            return reservation != null ? MapToDto(reservation) : null;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ReservationDto>> GetReservationsByUserIdAsync(Guid userId)
        {
            var reservations = await _reservationRepository.GetByUserIdAsync(userId);
            return reservations.Select(MapToDto);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ReservationDto>> GetReservationsByParkingSpotIdAsync(Guid parkingSpotId)
        {
            var reservations = await _reservationRepository.GetByParkingSpotIdAsync(parkingSpotId);
            return reservations.Select(MapToDto);
        }

        /// <inheritdoc/>
        public async Task<ReservationDto> CreateReservationAsync(CreateReservationDto createDto)
        {
            // Validate input
            if (createDto.EndTime <= createDto.StartTime)
            {
                throw new ArgumentException("End time must be after start time");
            }

            // Check if parking spot exists
            var parkingSpot = await _parkingSpotRepository.GetByIdAsync(createDto.ParkingSpotId);
            if (parkingSpot == null)
            {
                throw new KeyNotFoundException($"Parking spot with ID {createDto.ParkingSpotId} not found");
            }

            // Check if spot is available
            if (parkingSpot.Status != SpotStatus.Available)
            {
                throw new InvalidOperationException($"Parking spot {parkingSpot.SpotName} is not available");
            }

            // Check if user exists
            var user = await _userRepository.GetByIdAsync(createDto.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {createDto.UserId} not found");
            }

            // Check if car exists
            var car = await _carRepository.GetByIdAsync(createDto.CarId);
            if (car == null)
            {
                throw new KeyNotFoundException($"Car with ID {createDto.CarId} not found");
            }

            // Check if car belongs to user
            var isCarOwnedByUser = await _carRepository.IsCarOwnedByUserAsync(createDto.CarId, createDto.UserId);
            if (!isCarOwnedByUser)
            {
                throw new InvalidOperationException("The car does not belong to the specified user");
            }

            // Check for overlapping reservations
            var hasOverlap = await _reservationRepository.HasOverlappingReservationsAsync(
                createDto.ParkingSpotId, createDto.StartTime, createDto.EndTime);

            if (hasOverlap)
            {
                throw new InvalidOperationException("There is already a reservation for this parking spot during the specified time period");
            }

            // Create reservation
            var reservation = new Reservation
            {
                StartTime = createDto.StartTime,
                EndTime = createDto.EndTime,
                Status = ReservationStatus.Active,
                UserId = createDto.UserId,
                CarId = createDto.CarId,
                ParkingSpotId = createDto.ParkingSpotId
            };

            await _reservationRepository.AddAsync(reservation);
            await _reservationRepository.SaveChangesAsync();

            // Update parking spot status to Reserved
            parkingSpot.Status = SpotStatus.Reserved;
            await _parkingSpotRepository.SaveChangesAsync();

            // Reload the reservation with all details
            var createdReservation = await _reservationRepository.GetReservationWithDetailsAsync(reservation.Id);
            return MapToDto(createdReservation!);
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateReservationStatusAsync(Guid id, UpdateReservationStatusDto updateDto)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null)
            {
                return false;
            }

            // Only allow status changes if the reservation is currently active
            if (reservation.Status != ReservationStatus.Active)
            {
                throw new InvalidOperationException($"Cannot update status of a reservation that is already {reservation.Status}");
            }

            reservation.Status = updateDto.Status;
            await _reservationRepository.SaveChangesAsync();

            // If the reservation is completed or cancelled, update the parking spot status
            if (updateDto.Status == ReservationStatus.Completed || updateDto.Status == ReservationStatus.Cancelled)
            {
                var parkingSpot = await _parkingSpotRepository.GetByIdAsync(reservation.ParkingSpotId);
                if (parkingSpot != null)
                {
                    parkingSpot.Status = SpotStatus.Available;
                    await _parkingSpotRepository.SaveChangesAsync();
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> CancelReservationAsync(Guid id)
        {
            var updateDto = new UpdateReservationStatusDto { Status = ReservationStatus.Cancelled };
            return await UpdateReservationStatusAsync(id, updateDto);
        }

        /// <summary>
        /// Maps a Reservation entity to a ReservationDto
        /// </summary>
        private static ReservationDto MapToDto(Reservation reservation)
        {
            return new ReservationDto
            {
                Id = reservation.Id,
                StartTime = reservation.StartTime,
                EndTime = reservation.EndTime,
                Status = reservation.Status.ToString(),
                UserId = reservation.UserId,
                UserName = reservation.User?.FullName ?? string.Empty,
                CarId = reservation.CarId,
                LicensePlate = reservation.Car?.LicensePlate ?? string.Empty,
                ParkingSpotId = reservation.ParkingSpotId,
                SpotName = reservation.ParkingSpot?.SpotName ?? string.Empty
            };
        }
    }
}