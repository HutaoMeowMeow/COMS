namespace COMS.DTOs;

public class DashboardSummaryDto
{
    public int TotalCanals { get; set; }
    public int TotalSensors { get; set; }
    public int ActiveAlerts { get; set; }
    public int PendingReports { get; set; }
    public int TotalUsers { get; set; }
    public int UnreadNotifications { get; set; }
}

public class CanalStatusDto
{
    public Guid CanalId { get; set; }
    public string CanalName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public double? CurrentWaterLevel { get; set; }
    public string? CurrentRiskLevel { get; set; }
    public DateTime? LastReadingAt { get; set; }
}
