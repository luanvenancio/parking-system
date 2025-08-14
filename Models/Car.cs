using System.ComponentModel.DataAnnotations;

namespace ParkingSystem.Models
{
    public class Car
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(20)]
        public string LicensePlate { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Model { get; set; } = string.Empty;

        [MaxLength(30)]
        public string? Color { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public ICollection<ParkingSession> ParkingSessions { get; set; } = new List<ParkingSession>();
    }
}