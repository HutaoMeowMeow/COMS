using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COMS.Models;

public class ObstructionAlert
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid CanalId { get; set; }

    [Required]
    [MaxLength(50)]
    public string AlertType { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Severity { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Status { get; set; } = "Active";

    [MaxLength(100)]
    public string? ObstructionType { get; set; }

    public double? WaterLevelAtAlert { get; set; }

    public Guid? TriggeredByReadingId { get; set; }

    public DateTime TriggeredAt { get; set; } = DateTime.UtcNow;

    public DateTime? ResolvedAt { get; set; }

    [MaxLength(255)]
    public string? ResolutionNotes { get; set; }

    public Canal? Canal { get; set; }
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
