using COMS.Data;
using COMS.DTOs;
using COMS.Models;
using Microsoft.EntityFrameworkCore;

namespace COMS.Services;

public class FloodRiskService : IFloodRiskService
{
    private readonly ApplicationDbContext _context;

    public FloodRiskService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FloodRiskResponseDto> CreateAssessmentAsync(Guid canalId, string riskLevel, double riskScore, string predictionDetails, string? modelVersion = null)
    {
        var canal = await _context.Canals.FindAsync(canalId);
        if (canal == null)
            throw new InvalidOperationException("Canal not found.");

        var assessment = new FloodRiskAssessment
        {
            Id = Guid.NewGuid(),
            CanalId = canalId,
            RiskLevel = riskLevel,
            RiskScore = riskScore,
            PredictionDetails = predictionDetails,
            ModelVersion = modelVersion,
            AssessmentTimestamp = DateTime.UtcNow,
            ValidUntil = DateTime.UtcNow.AddDays(7)
        };

        _context.FloodRiskAssessments.Add(assessment);
        await _context.SaveChangesAsync();

        return MapToResponse(assessment);
    }

    public async Task<IEnumerable<FloodRiskResponseDto>> GetAllAsync()
    {
        return await _context.FloodRiskAssessments
            .OrderByDescending(fra => fra.AssessmentTimestamp)
            .Select(fra => MapToResponse(fra))
            .ToListAsync();
    }

    public async Task<IEnumerable<FloodRiskResponseDto>> GetByCanalAsync(Guid canalId)
    {
        return await _context.FloodRiskAssessments
            .Where(fra => fra.CanalId == canalId)
            .OrderByDescending(fra => fra.AssessmentTimestamp)
            .Select(fra => MapToResponse(fra))
            .ToListAsync();
    }

    public async Task<IEnumerable<RiskAnalyticsDto>> GetAnalyticsAsync()
    {
        var canals = await _context.Canals.ToListAsync();
        var canalIds = canals.Select(c => c.Id).ToArray();

        var alerts = await _context.ObstructionAlerts
            .Where(a => canalIds.Contains(a.CanalId))
            .ToListAsync();

        var reports = await _context.CommunityReports
            .Where(r => canalIds.Contains(r.CanalId))
            .ToListAsync();

        var readings = await _context.SensorReadings
            .Where(sr => canalIds.Contains(sr.CanalId))
            .ToListAsync();

        var assessments = await _context.FloodRiskAssessments
            .Where(fra => canalIds.Contains(fra.CanalId))
            .ToListAsync();

        return canals.Select(canal =>
        {
            var canalAlerts = alerts.Where(a => a.CanalId == canal.Id).ToList();
            var canalReports = reports.Where(r => r.CanalId == canal.Id).ToList();
            var canalReadings = readings.Where(sr => sr.CanalId == canal.Id).ToList();
            var canalAssessments = assessments.Where(fra => fra.CanalId == canal.Id).ToList();

            var latestReadings = canalReadings
                .OrderByDescending(sr => sr.ReadingTimestamp)
                .Take(10)
                .ToList();
            var avgWaterLevel = latestReadings.Any() ? latestReadings.Average(sr => sr.WaterLevel) : 0;

            var latestRisk = canalAssessments
                .OrderByDescending(fra => fra.AssessmentTimestamp)
                .FirstOrDefault();

            return new RiskAnalyticsDto
            {
                CanalId = canal.Id,
                CanalName = canal.Name,
                TotalAlerts = canalAlerts.Count,
                ActiveAlerts = canalAlerts.Count(a => a.Status == "Active"),
                ResolvedAlerts = canalAlerts.Count(a => a.Status == "Resolved"),
                PendingReports = canalReports.Count(r => r.Status == "Pending"),
                AverageWaterLevel = Math.Round(avgWaterLevel, 2),
                CurrentRiskLevel = latestRisk?.RiskLevel ?? "Low",
                LastAssessment = canalAssessments.Any() ? canalAssessments.Max(fra => fra.AssessmentTimestamp) : DateTime.MinValue
            };
        }).ToList();
    }

    public async Task<RiskAnalyticsDto?> GetAnalyticsByCanalAsync(Guid canalId)
    {
        var canal = await _context.Canals.FindAsync(canalId);
        if (canal == null) return null;

        var totalAlerts = await _context.ObstructionAlerts.CountAsync(a => a.CanalId == canalId);
        var activeAlerts = await _context.ObstructionAlerts.CountAsync(a => a.CanalId == canalId && a.Status == "Active");
        var resolvedAlerts = await _context.ObstructionAlerts.CountAsync(a => a.CanalId == canalId && a.Status == "Resolved");
        var pendingReports = await _context.CommunityReports.CountAsync(r => r.CanalId == canalId && r.Status == "Pending");

        var latestReadings = await _context.SensorReadings
            .Where(sr => sr.CanalId == canalId)
            .OrderByDescending(sr => sr.ReadingTimestamp)
            .Take(10)
            .ToListAsync();

        var avgWaterLevel = latestReadings.Any() ? latestReadings.Average(sr => sr.WaterLevel) : 0;

        var latestRisk = await _context.FloodRiskAssessments
            .Where(fra => fra.CanalId == canalId)
            .OrderByDescending(fra => fra.AssessmentTimestamp)
            .Select(fra => fra.RiskLevel)
            .FirstOrDefaultAsync();

        return new RiskAnalyticsDto
        {
            CanalId = canal.Id,
            CanalName = canal.Name,
            TotalAlerts = totalAlerts,
            ActiveAlerts = activeAlerts,
            ResolvedAlerts = resolvedAlerts,
            PendingReports = pendingReports,
            AverageWaterLevel = Math.Round(avgWaterLevel, 2),
            CurrentRiskLevel = latestRisk ?? "Low",
            LastAssessment = canal.FloodRiskAssessments.Any() ? canal.FloodRiskAssessments.Max(fra => fra.AssessmentTimestamp) : DateTime.MinValue
        };
    }

    private static FloodRiskResponseDto MapToResponse(FloodRiskAssessment assessment)
    {
        return new FloodRiskResponseDto
        {
            Id = assessment.Id,
            CanalId = assessment.CanalId,
            CanalName = assessment.Canal?.Name ?? string.Empty,
            RiskLevel = assessment.RiskLevel,
            RiskScore = assessment.RiskScore,
            PredictionDetails = assessment.PredictionDetails,
            ModelVersion = assessment.ModelVersion,
            AssessmentTimestamp = assessment.AssessmentTimestamp,
            ValidUntil = assessment.ValidUntil
        };
    }
}
