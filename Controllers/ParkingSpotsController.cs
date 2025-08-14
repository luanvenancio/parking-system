using Microsoft.AspNetCore.Mvc;
using ParkingSystem.DTOs;
using ParkingSystem.Models;
using ParkingSystem.Services.Interfaces;

namespace ParkingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParkingSpotsController : ControllerBase
    {
        private readonly IParkingSpotService _parkingSpotService;

        public ParkingSpotsController(IParkingSpotService parkingSpotService)
        {
            _parkingSpotService = parkingSpotService;
        }

        /// <summary>
        /// Get all parking spots
        /// </summary>
        /// <returns>List of parking spots</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ParkingSpotDto>>> GetParkingSpots()
        {
            var spots = await _parkingSpotService.GetAllParkingSpotsAsync();
            return Ok(spots);
        }

        /// <summary>
        /// Get all parking spots for a specific parking lot
        /// </summary>
        /// <param name="parkingLotId">Parking lot id</param>
        /// <returns>List of parking spots</returns>
        [HttpGet("byLot/{parkingLotId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ParkingSpotDto>>> GetParkingSpotsByLot(Guid parkingLotId)
        {
            var spots = await _parkingSpotService.GetParkingSpotsByLotIdAsync(parkingLotId);
            return Ok(spots);
        }

        /// <summary>
        /// Get a specific parking spot by id
        /// </summary>
        /// <param name="id">Parking spot id</param>
        /// <returns>Parking spot details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ParkingSpotDto>> GetParkingSpot(Guid id)
        {
            var spot = await _parkingSpotService.GetParkingSpotByIdAsync(id);

            if (spot == null)
            {
                return NotFound();
            }

            return Ok(spot);
        }

        /// <summary>
        /// Create a new parking spot
        /// </summary>
        /// <param name="parkingLotId">Parking lot id</param>
        /// <param name="request">Parking spot creation data</param>
        /// <returns>Created parking spot</returns>
        [HttpPost("parkingLot/{parkingLotId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ParkingSpotDto>> CreateParkingSpot(Guid parkingLotId, [FromBody] CreateParkingSpotRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdSpot = await _parkingSpotService.CreateParkingSpotAsync(
                    parkingLotId,
                    request.SpotTypeId,
                    request.SpotName,
                    request.FloorLevel);

                return CreatedAtAction(nameof(GetParkingSpot), new { id = createdSpot.Id }, createdSpot);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update parking spot status
        /// </summary>
        /// <param name="id">Parking spot id</param>
        /// <param name="request">Status update request</param>
        /// <returns>No content</returns>
        [HttpPatch("{id}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateParkingSpotStatus(Guid id, [FromBody] UpdateSpotStatusRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!Enum.TryParse<SpotStatus>(request.Status, true, out var status))
            {
                return BadRequest($"Invalid status value. Valid values are: {string.Join(", ", Enum.GetNames<SpotStatus>())}");
            }

            try
            {
                var result = await _parkingSpotService.UpdateParkingSpotStatusAsync(id, status);

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
        /// Delete a parking spot
        /// </summary>
        /// <param name="id">Parking spot id</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteParkingSpot(Guid id)
        {
            try
            {
                var result = await _parkingSpotService.DeleteParkingSpotAsync(id);

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

    public class CreateParkingSpotRequest
    {
        public string SpotName { get; set; } = string.Empty;
        public int FloorLevel { get; set; }
        public Guid SpotTypeId { get; set; }
    }

    public class UpdateSpotStatusRequest
    {
        public string Status { get; set; } = string.Empty;
    }
}