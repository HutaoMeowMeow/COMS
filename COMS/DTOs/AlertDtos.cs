namespace COMS.DTOs;

public class CreateAlertDto
{
    public Guid CanalId { get; set; }
    public string AlertType { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ObstructionType { get; set; }
    public double? WaterLevelAtAlert { get; set; }
}

public class UpdateAlertDto
{
    public string Status { get; set; } = "Active";
    public string? ResolutionNotes { get; set; }
}

public class AlertResponseDto
{
    public Guid Id { get; set; }
    public Guid CanalId { get; set; }
    public string CanalName { get; set; } = string.Empty;
    public string AlertType { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? ObstructionType { get; set; }
    public double? WaterLevelAtAlert { get; set; }
    public DateTime TriggeredAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string? ResolutionNotes { get; set; }
}
