namespace COMS.DTOs;

public class CreateAssessmentDto
{
    public string RiskLevel { get; set; } = string.Empty;
    public double RiskScore { get; set; }
    public string PredictionDetails { get; set; } = string.Empty;
    public string? ModelVersion { get; set; }
}
