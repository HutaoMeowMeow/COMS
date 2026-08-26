using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COMS.Models;

public class SensorReading
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid SensorId { get; set; }

    [Required]
    public Guid CanalId { get; set; }

    public double WaterLevel { get; set; }

    public double FlowRate { get; set; }

    public double? DebrisLevel { get; set; }

    public double? Turbidity { get; set; }

    public double? Temperature { get; set; }

    [MaxLength(50)]
    public string ReadingStatus { get; set; } = "Normal";

    public DateTime ReadingTimestamp { get; set; } = DateTime.UtcNow;

    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;

    public Sensor? Sensor { get; set; }
    public Canal? Canal { get; set; }
}
