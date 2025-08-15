using System.ComponentModel.DataAnnotations;

namespace ParkingSystem.DTOs
{
    public class CreateParkingLotDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(100)]
        public string? OperatingHours { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }
    }

    public class SpotTypeDetailDto
    {
        public string TypeName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int TotalCount { get; set; }
        public int AvailableCount { get; set; }
        public int OccupiedCount { get; set; }
        public decimal? HourlyRate { get; set; }
        public decimal? DailyMaxCap { get; set; }
        public string? PricingDetails { get; set; }
    }

    public class ParkingSpotSummaryDto
    {
        public Guid Id { get; set; }
        public string SpotName { get; set; } = string.Empty;
        public int FloorLevel { get; set; }
        public string Status { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;
    }
    public class ParkingLotDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? OperatingHours { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public int TotalSpots { get; set; }
        public int AvailableSpots { get; set; }
        public int OccupiedSpots { get; set; }

        public double OccupancyPercentage { get; set; }

        public IEnumerable<SpotTypeDetailDto> SpotTypesDetails { get; set; } = new List<SpotTypeDetailDto>();

        public IEnumerable<ParkingSpotSummaryDto> Spots { get; set; } = new List<ParkingSpotSummaryDto>();
    }

    public class ParkingLotDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? OperatingHours { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public int TotalSpots { get; set; }
        public int AvailableSpots { get; set; }
        public int OccupiedSpots { get; set; }
        public int ReservedSpots { get; set; }
        public int MaintenanceSpots { get; set; }

        public double OccupancyPercentage { get; set; }
        public double AvailabilityPercentage { get; set; }

        public IEnumerable<SpotTypeDetailDto> SpotTypesDetails { get; set; } = new List<SpotTypeDetailDto>();

        public decimal? MinHourlyRate { get; set; }
        public decimal? MaxHourlyRate { get; set; }
        public bool HasDifferentPricing { get; set; }
    }
}