using System.ComponentModel.DataAnnotations;

namespace ParkingSystem.DTOs
{
    public class CreateParkingSpotDto
    {
        [Required]
        [MaxLength(50)]
        public string SpotName { get; set; } = string.Empty;
        
        public int FloorLevel { get; set; }
        
        [Required]
        public Guid ParkingLotId { get; set; }
        
        [Required]
        public Guid SpotTypeId { get; set; }
    }

    public class ParkingSpotDto
    {
        public Guid Id { get; set; }
        public string SpotName { get; set; } = string.Empty;
        public int FloorLevel { get; set; }
        public string Status { get; set; } = string.Empty;
        public string SpotTypeName { get; set; } = string.Empty;
        public string? SpotTypeDescription { get; set; }
        
        // Informações do estacionamento
        public Guid ParkingLotId { get; set; }
        public string ParkingLotName { get; set; } = string.Empty;
        public string ParkingLotAddress { get; set; } = string.Empty;
        
        // Informações de preço
        public decimal? HourlyRate { get; set; }
        public decimal? DailyMaxCap { get; set; }
        public string? PricingDetails { get; set; }
        
        // Disponibilidade futura
        public DateTime? NextAvailableTime { get; set; }
        public bool IsAvailableNow { get; set; }
        public bool HasUpcomingReservations { get; set; }
        
        // Estatísticas
        public double? AverageOccupancyRate { get; set; }
        public TimeSpan? AverageSessionDuration { get; set; }
    }

    public class ParkingSpotDetailDto : ParkingSpotDto
    {
        // Reservas futuras (próximas 24 horas)
        public IEnumerable<UpcomingReservationDto> UpcomingReservations { get; set; } = new List<UpcomingReservationDto>();
        
        // Histórico recente de ocupação
        public IEnumerable<RecentSessionDto> RecentSessions { get; set; } = new List<RecentSessionDto>();
    }

    public class UpcomingReservationDto
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
    }

    public class RecentSessionDto
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan? Duration { get; set; }
        public decimal? FinalCost { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
    }
}