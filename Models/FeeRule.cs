using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingSystem.Models
{
    public enum ChargeType
    {
        Hourly,
        PerMinute,
        FlatRate
    }

    public class FeeRule
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public int Priority { get; set; }

        public string? DaysOfWeek { get; set; }

        public TimeOnly? TimeStart { get; set; }
        public TimeOnly? TimeEnd { get; set; }

        [Required]
        public ChargeType ChargeType { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ChargeAmount { get; set; }

        [Required]
        public Guid FeeId { get; set; }
        [ForeignKey("FeeId")]
        public Fee? Fee { get; set; }
    }
}