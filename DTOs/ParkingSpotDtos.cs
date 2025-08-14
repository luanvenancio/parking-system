namespace ParkingSystem.DTOs
{
    public class ParkingSpotDto
    {
        public Guid Id { get; set; }
        public string SpotName { get; set; } = string.Empty;
        public int FloorLevel { get; set; }
        public string Status { get; set; } = string.Empty;
        public string SpotTypeName { get; set; } = string.Empty;
    }
}