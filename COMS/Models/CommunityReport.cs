using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COMS.Models;

public class CommunityReport
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid CanalId { get; set; }

    [Required]
    public Guid ReportedByUserId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ReportType { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Pending";

    [MaxLength(255)]
    public string? VerifiedBy { get; set; }

    public DateTime ReportedAt { get; set; } = DateTime.UtcNow;

    public DateTime? VerifiedAt { get; set; }

    public DateTime? ResolvedAt { get; set; }

    [MaxLength(255)]
    public string? ResolutionNotes { get; set; }

    public Canal? Canal { get; set; }
    public User? ReportedByUser { get; set; }
}
