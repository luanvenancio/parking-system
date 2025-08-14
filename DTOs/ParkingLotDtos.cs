using System.ComponentModel.DataAnnotations;

namespace ParkingSystem.DTOs
{
    public class CreateParkingLotDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }
    }

    public class ParkingLotDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int TotalSpots { get; set; }
    }
}