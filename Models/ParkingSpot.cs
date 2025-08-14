using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingSystem.Models
{
    public enum SpotStatus
    {
        Available,
        Occupied,
        Maintenance,
        Reserved
    }

    public class ParkingSpot
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(20)]
        public string SpotName { get; set; } = string.Empty;

        public int FloorLevel { get; set; }

        [Required]
        public SpotStatus Status { get; set; }

        [Required]
        public Guid ParkingLotId { get; set; }

        [ForeignKey("ParkingLotId")]
        public ParkingLot? ParkingLot { get; set; }

        [Required]
        public Guid SpotTypeId { get; set; }

        [ForeignKey("SpotTypeId")]
        public SpotType? SpotType { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public ICollection<ParkingSession> ParkingSessions { get; set; } = new List<ParkingSession>();
    }
}