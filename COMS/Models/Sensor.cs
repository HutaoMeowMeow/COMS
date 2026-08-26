using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COMS.Models;

public class Sensor
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string SensorCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string SensorType { get; set; } = string.Empty;

    [Required]
    public Guid CanalId { get; set; }

    [MaxLength(100)]
    public string? LocationDescription { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    [MaxLength(50)]
    public string CommunicationProtocol { get; set; } = "MQTT";

    [MaxLength(50)]
    public string Status { get; set; } = "Online";

    public DateTime? LastReadingAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public Canal? Canal { get; set; }
    public ICollection<SensorReading> SensorReadings { get; set; } = new List<SensorReading>();
}
