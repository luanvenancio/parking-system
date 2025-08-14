using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingSystem.Models
{
    public enum PaymentTransactionStatus
    {
        Successful,
        Failed,
        Pending,
        Refunded
    }

    public class Payment
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal AmountPaid { get; set; }

        public string? PaymentMethod { get; set; }

        public string? TransactionId { get; set; }

        [Required]
        public PaymentTransactionStatus Status { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public Guid ParkingSessionId { get; set; }
        [ForeignKey("ParkingSessionId")]
        public ParkingSession? ParkingSession { get; set; }

        [Required]
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}