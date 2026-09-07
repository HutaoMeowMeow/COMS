namespace COMS.DTOs;

public class CreateSensorDto
{
    public string SensorCode { get; set; } = string.Empty;
    public string SensorType { get; set; } = string.Empty;
    public Guid CanalId { get; set; }
    public string? LocationDescription { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string CommunicationProtocol { get; set; } = "MQTT";
}

public class UpdateSensorDto
{
    public string SensorCode { get; set; } = string.Empty;
    public string SensorType { get; set; } = string.Empty;
    public string? LocationDescription { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string CommunicationProtocol { get; set; } = "MQTT";
    public string Status { get; set; } = "Online";
}

public class SensorResponseDto
{
    public Guid Id { get; set; }
    public string SensorCode { get; set; } = string.Empty;
    public string SensorType { get; set; } = string.Empty;
    public Guid CanalId { get; set; }
    public string? CanalName { get; set; }
    public string? LocationDescription { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string CommunicationProtocol { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? LastReadingAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SensorReadingIngestionDto
{
    public string SensorCode { get; set; } = string.Empty;
    public double WaterLevel { get; set; }
    public double FlowRate { get; set; }
    public double? DebrisLevel { get; set; }
    public double? Turbidity { get; set; }
    public double? Temperature { get; set; }
    public DateTime ReadingTimestamp { get; set; }
}
