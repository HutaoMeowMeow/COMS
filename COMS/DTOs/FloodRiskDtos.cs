namespace COMS.DTOs;

public class FloodRiskResponseDto
{
    public Guid Id { get; set; }
    public Guid CanalId { get; set; }
    public string CanalName { get; set; } = string.Empty;
    public string RiskLevel { get; set; } = string.Empty;
    public double RiskScore { get; set; }
    public string PredictionDetails { get; set; } = string.Empty;
    public string? ModelVersion { get; set; }
    public DateTime AssessmentTimestamp { get; set; }
    public DateTime ValidUntil { get; set; }
}

public class RiskAnalyticsDto
{
    public Guid CanalId { get; set; }
    public string CanalName { get; set; } = string.Empty;
    public int TotalAlerts { get; set; }
    public int ActiveAlerts { get; set; }
    public int ResolvedAlerts { get; set; }
    public int PendingReports { get; set; }
    public double AverageWaterLevel { get; set; }
    public string CurrentRiskLevel { get; set; } = "Low";
    public DateTime LastAssessment { get; set; }
}
