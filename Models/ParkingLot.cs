using System.ComponentModel.DataAnnotations;

namespace ParkingSystem.Models
{
    public class ParkingLot
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Address { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ICollection<ParkingSpot> ParkingSpots { get; set; } = new List<ParkingSpot>();
    }
}