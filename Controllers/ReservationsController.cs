using Microsoft.AspNetCore.Mvc;
using ParkingSystem.DTOs;
using ParkingSystem.Services.Interfaces;

namespace ParkingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        /// <summary>
        /// Get all reservations
        /// </summary>
        /// <returns>List of all reservations</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetAllReservations()
        {
            var reservations = await _reservationService.GetAllReservationsAsync();
            return Ok(reservations);
        }

        /// <summary>
        /// Get a specific reservation by id
        /// </summary>
        /// <param name="id">Reservation id</param>
        /// <returns>Reservation details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationDto>> GetReservation(Guid id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        /// <summary>
        /// Get all reservations for a specific user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>List of user's reservations</returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetUserReservations(Guid userId)
        {
            var reservations = await _reservationService.GetReservationsByUserIdAsync(userId);
            return Ok(reservations);
        }

        /// <summary>
        /// Get all reservations for a specific parking spot
        /// </summary>
        /// <param name="spotId">Parking spot id</param>
        /// <returns>List of reservations for the spot</returns>
        [HttpGet("spot/{spotId}")]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetSpotReservations(Guid spotId)
        {
            var reservations = await _reservationService.GetReservationsByParkingSpotIdAsync(spotId);
            return Ok(reservations);
        }

        /// <summary>
        /// Create a new reservation
        /// </summary>
        /// <param name="createDto">Reservation data</param>
        /// <returns>Created reservation</returns>
        [HttpPost]
        public async Task<ActionResult<ReservationDto>> CreateReservation(CreateReservationDto createDto)
        {
            try
            {
                var reservation = await _reservationService.CreateReservationAsync(createDto);
                return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update reservation status
        /// </summary>
        /// <param name="id">Reservation id</param>
        /// <param name="updateDto">Updated status</param>
        /// <returns>No content if successful</returns>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateReservationStatus(Guid id, UpdateReservationStatusDto updateDto)
        {
            try
            {
                var result = await _reservationService.UpdateReservationStatusAsync(id, updateDto);
                if (!result)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Cancel a reservation
        /// </summary>
        /// <param name="id">Reservation id</param>
        /// <returns>No content if successful</returns>
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelReservation(Guid id)
        {
            try
            {
                var result = await _reservationService.CancelReservationAsync(id);
                if (!result)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}