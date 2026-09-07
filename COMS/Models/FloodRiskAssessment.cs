using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COMS.Models;

public class FloodRiskAssessment
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid CanalId { get; set; }

    [Required]
    [MaxLength(50)]
    public string RiskLevel { get; set; } = string.Empty;

    [Range(0, 100)]
    public double RiskScore { get; set; }

    [Required]
    public string PredictionDetails { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? ModelVersion { get; set; }

    public DateTime AssessmentTimestamp { get; set; } = DateTime.UtcNow;

    public DateTime ValidUntil { get; set; }

    public Canal? Canal { get; set; }
}
