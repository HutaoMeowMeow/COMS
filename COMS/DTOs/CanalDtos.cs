namespace COMS.DTOs;

public class CreateCanalDto
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string? Barangay { get; set; }
    public string? Municipality { get; set; }
    public string? Province { get; set; }
    public double LengthMeters { get; set; }
    public double WidthMeters { get; set; }
    public double DepthMeters { get; set; }
    public double NormalWaterLevel { get; set; }
    public double WarningWaterLevel { get; set; }
    public double CriticalWaterLevel { get; set; }
}

public class UpdateCanalDto
{
<<<<<<< HEAD
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string? Barangay { get; set; }
    public string? Municipality { get; set; }
    public string? Province { get; set; }
    public double LengthMeters { get; set; }
    public double WidthMeters { get; set; }
    public double DepthMeters { get; set; }
    public double NormalWaterLevel { get; set; }
    public double WarningWaterLevel { get; set; }
    public double CriticalWaterLevel { get; set; }
    public string Status { get; set; } = "Normal";
=======
    public string? Name { get; set; }
    public string? Location { get; set; }
    public string? Barangay { get; set; }
    public string? Municipality { get; set; }
    public string? Province { get; set; }
    public double? LengthMeters { get; set; }
    public double? WidthMeters { get; set; }
    public double? DepthMeters { get; set; }
    public double? NormalWaterLevel { get; set; }
    public double? WarningWaterLevel { get; set; }
    public double? CriticalWaterLevel { get; set; }
    public string? Status { get; set; }
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
}

public class CanalResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string? Barangay { get; set; }
    public string? Municipality { get; set; }
    public string? Province { get; set; }
    public string Status { get; set; } = "Normal";
    public double LengthMeters { get; set; }
    public double WidthMeters { get; set; }
    public double DepthMeters { get; set; }
    public double NormalWaterLevel { get; set; }
    public double WarningWaterLevel { get; set; }
    public double CriticalWaterLevel { get; set; }
    public int SensorCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
