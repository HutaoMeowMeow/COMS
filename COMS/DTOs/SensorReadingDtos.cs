namespace COMS.DTOs;

public class SensorReadingResponseDto
{
    public Guid Id { get; set; }
    public Guid SensorId { get; set; }
    public string SensorCode { get; set; } = string.Empty;
    public Guid CanalId { get; set; }
    public string CanalName { get; set; } = string.Empty;
    public double WaterLevel { get; set; }
    public double FlowRate { get; set; }
    public double? DebrisLevel { get; set; }
    public double? Turbidity { get; set; }
    public double? Temperature { get; set; }
    public string ReadingStatus { get; set; } = string.Empty;
    public DateTime ReadingTimestamp { get; set; }
    public DateTime ReceivedAt { get; set; }
}
