using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingSystem.Models
{
    public enum PaymentStatus
    {
        Unpaid,
        Paid,
        Pending,
        Failed
    }

    public class ParkingSession
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? FinalCost { get; set; }

        [Required]
        public PaymentStatus PaymentStatus { get; set; }

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

        public Guid? ReservationId { get; set; }
        [ForeignKey("ReservationId")]
        public Reservation? Reservation { get; set; }

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}