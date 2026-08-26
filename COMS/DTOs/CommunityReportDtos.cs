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

<<<<<<< HEAD
public class CompleteReportDto
{
    public string? CompletionImageUrl { get; set; }
    public string? CompletionRemarks { get; set; }
}

=======
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
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
<<<<<<< HEAD
    public string? CompletionImageUrl { get; set; }
    public string? CompletionRemarks { get; set; }
    public Guid? CompletedByUserId { get; set; }
    public string? CompletedByUserName { get; set; }
    public DateTime? CompletedAt { get; set; }
=======
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
}
