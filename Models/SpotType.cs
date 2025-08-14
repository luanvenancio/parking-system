using System.ComponentModel.DataAnnotations;

namespace ParkingSystem.Models
{
    public class SpotType
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ICollection<ParkingSpot> ParkingSpots { get; set; } = new List<ParkingSpot>();
        public Fee? Fee { get; set; }
    }
}