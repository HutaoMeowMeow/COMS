using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COMS.Models;

public class Canal
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Location { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Barangay { get; set; }

    [MaxLength(100)]
    public string? Municipality { get; set; }

    [MaxLength(100)]
    public string? Province { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Normal";

    public double LengthMeters { get; set; }

    public double WidthMeters { get; set; }

    public double DepthMeters { get; set; }

    public double NormalWaterLevel { get; set; }

    public double WarningWaterLevel { get; set; }

    public double CriticalWaterLevel { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public ICollection<Sensor> Sensors { get; set; } = new List<Sensor>();
    public ICollection<SensorReading> SensorReadings { get; set; } = new List<SensorReading>();
    public ICollection<ObstructionAlert> ObstructionAlerts { get; set; } = new List<ObstructionAlert>();
    public ICollection<CommunityReport> CommunityReports { get; set; } = new List<CommunityReport>();
    public ICollection<FloodRiskAssessment> FloodRiskAssessments { get; set; } = new List<FloodRiskAssessment>();
}
