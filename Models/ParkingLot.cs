using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [MaxLength(100)]
        public string? OperatingHours { get; set; }

        [Column(TypeName = "decimal(9, 6)")]
        public decimal? Latitude { get; set; }

        [Column(TypeName = "decimal(9, 6)")]
        public decimal? Longitude { get; set; }

        public ICollection<ParkingSpot> ParkingSpots { get; set; } = new List<ParkingSpot>();
    }
}