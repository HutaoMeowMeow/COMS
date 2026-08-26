using COMS.Data;
using COMS.DTOs;
using COMS.Models;
<<<<<<< HEAD
=======
using Microsoft.EntityFrameworkCore;
>>>>>>> db46004de7d488abfb831db9c6cd307518689719

namespace COMS.Services;

public interface IFloodRiskService
{
    Task<FloodRiskResponseDto> CreateAssessmentAsync(Guid canalId, string riskLevel, double riskScore, string predictionDetails, string? modelVersion = null);
    Task<IEnumerable<FloodRiskResponseDto>> GetAllAsync();
    Task<IEnumerable<FloodRiskResponseDto>> GetByCanalAsync(Guid canalId);
    Task<IEnumerable<RiskAnalyticsDto>> GetAnalyticsAsync();
    Task<RiskAnalyticsDto?> GetAnalyticsByCanalAsync(Guid canalId);
}
