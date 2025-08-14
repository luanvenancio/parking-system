using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingSystem.Models
{
    public class Fee
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? DailyMaxCap { get; set; }

        [Required]
        public Guid SpotTypeId { get; set; }

        [ForeignKey("SpotTypeId")]
        public SpotType? SpotType { get; set; }

        public ICollection<FeeRule> FeeRules { get; set; } = new List<FeeRule>();
    }
}