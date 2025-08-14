using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingSystem.Models
{
    public enum ReservationStatus
    {
        Active,
        Cancelled,
        Completed
    }

    public class Reservation
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public ReservationStatus Status { get; set; }

        [Required]
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required]
        public Guid CarId { get; set; }
        [ForeignKey("CarId")]
        public Car? Car { get; set; }

        [Required]
        public Guid ParkingSpotId { get; set; }
        [ForeignKey("ParkingSpotId")]
        public ParkingSpot? ParkingSpot { get; set; }
    }
}