namespace COMS.DTOs;

public class CreateCommunityReportDto
{
    public Guid CanalId { get; set; }
    public string ReportType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
}

public class UpdateCommunityReportDto
{
    public string Status { get; set; } = "Pending";
    public string? VerifiedBy { get; set; }
    public string? ResolutionNotes { get; set; }
}

public class CommunityReportResponseDto
{
    public Guid Id { get; set; }
    public Guid CanalId { get; set; }
    public string CanalName { get; set; } = string.Empty;
    public Guid ReportedByUserId { get; set; }
    public string ReportedByUserName { get; set; } = string.Empty;
    public string ReportType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? VerifiedBy { get; set; }
    public DateTime ReportedAt { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string? ResolutionNotes { get; set; }
}
