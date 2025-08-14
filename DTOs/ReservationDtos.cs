using System.ComponentModel.DataAnnotations;
using ParkingSystem.Models;

namespace ParkingSystem.DTOs
{
    public class CreateReservationDto
    {
        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid CarId { get; set; }

        [Required]
        public Guid ParkingSpotId { get; set; }
    }

    public class UpdateReservationStatusDto
    {
        [Required]
        public ReservationStatus Status { get; set; }
    }

    public class ReservationDto
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public Guid CarId { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public Guid ParkingSpotId { get; set; }
        public string SpotName { get; set; } = string.Empty;
    }
}