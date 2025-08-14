using System.ComponentModel.DataAnnotations;

namespace ParkingSystem.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public ICollection<Car> Cars { get; set; } = new List<Car>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public ICollection<ParkingSession> ParkingSessions { get; set; } = new List<ParkingSession>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}