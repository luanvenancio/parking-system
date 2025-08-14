using Microsoft.AspNetCore.Mvc;
using ParkingSystem.DTOs;
using ParkingSystem.Services.Interfaces;

namespace ParkingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParkingLotsController : ControllerBase
    {
        private readonly IParkingLotService _parkingLotService;

        public ParkingLotsController(IParkingLotService parkingLotService)
        {
            _parkingLotService = parkingLotService;
        }

        /// <summary>
        /// Get all parking lots
        /// </summary>
        /// <returns>List of parking lots</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ParkingLotDto>>> GetParkingLots()
        {
            var parkingLots = await _parkingLotService.GetAllParkingLotsAsync();
            return Ok(parkingLots);
        }

        /// <summary>
        /// Get a specific parking lot by id
        /// </summary>
        /// <param name="id">Parking lot id</param>
        /// <returns>Parking lot details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ParkingLotDto>> GetParkingLot(Guid id)
        {
            var parkingLot = await _parkingLotService.GetParkingLotByIdAsync(id);

            if (parkingLot == null)
            {
                return NotFound();
            }

            return Ok(parkingLot);
        }

        /// <summary>
        /// Create a new parking lot
        /// </summary>
        /// <param name="createDto">Parking lot data</param>
        /// <returns>Created parking lot</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ParkingLotDto>> CreateParkingLot(CreateParkingLotDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdParkingLot = await _parkingLotService.CreateParkingLotAsync(createDto);

            return CreatedAtAction(nameof(GetParkingLot), new { id = createdParkingLot.Id }, createdParkingLot);
        }

        /// <summary>
        /// Update an existing parking lot
        /// </summary>
        /// <param name="id">Parking lot id</param>
        /// <param name="updateDto">Updated parking lot data</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateParkingLot(Guid id, CreateParkingLotDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _parkingLotService.UpdateParkingLotAsync(id, updateDto);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Delete a parking lot
        /// </summary>
        /// <param name="id">Parking lot id</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteParkingLot(Guid id)
        {
            var result = await _parkingLotService.DeleteParkingLotAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}