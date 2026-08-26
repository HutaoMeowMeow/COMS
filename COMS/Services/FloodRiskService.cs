using COMS.Data;
using COMS.DTOs;
using COMS.Models;
<<<<<<< HEAD
using Google.Cloud.Firestore;
=======
using Microsoft.EntityFrameworkCore;
>>>>>>> db46004de7d488abfb831db9c6cd307518689719

namespace COMS.Services;

public class FloodRiskService : IFloodRiskService
{
<<<<<<< HEAD
    private readonly IFirestoreRepository<FloodRiskAssessment> _riskRepo;
    private readonly IFirestoreRepository<Canal> _canalRepo;
    private readonly IFirestoreRepository<ObstructionAlert> _alertRepo;
    private readonly IFirestoreRepository<CommunityReport> _reportRepo;
    private readonly IFirestoreRepository<SensorReading> _readingRepo;

    public FloodRiskService(
        IFirestoreRepository<FloodRiskAssessment> riskRepo,
        IFirestoreRepository<Canal> canalRepo,
        IFirestoreRepository<ObstructionAlert> alertRepo,
        IFirestoreRepository<CommunityReport> reportRepo,
        IFirestoreRepository<SensorReading> readingRepo)
    {
        _riskRepo = riskRepo;
        _canalRepo = canalRepo;
        _alertRepo = alertRepo;
        _reportRepo = reportRepo;
        _readingRepo = readingRepo;
=======
    private readonly ApplicationDbContext _context;

    public FloodRiskService(ApplicationDbContext context)
    {
        _context = context;
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    public async Task<FloodRiskResponseDto> CreateAssessmentAsync(Guid canalId, string riskLevel, double riskScore, string predictionDetails, string? modelVersion = null)
    {
<<<<<<< HEAD
        var canal = await _canalRepo.GetByIdAsync(canalId.ToString());
=======
        var canal = await _context.Canals.FindAsync(canalId);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
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

<<<<<<< HEAD
        await _riskRepo.CreateAsync(assessment);
        return await MapToResponseAsync(assessment);
=======
        _context.FloodRiskAssessments.Add(assessment);
        await _context.SaveChangesAsync();

        return MapToResponse(assessment);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    public async Task<IEnumerable<FloodRiskResponseDto>> GetAllAsync()
    {
<<<<<<< HEAD
        var assessments = await _riskRepo.QueryAsync(q => q.OrderByDescending("AssessmentTimestamp"));
        return await Task.WhenAll(assessments.Select(MapToResponseAsync));
=======
        return await _context.FloodRiskAssessments
            .OrderByDescending(fra => fra.AssessmentTimestamp)
            .Select(fra => MapToResponse(fra))
            .ToListAsync();
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    public async Task<IEnumerable<FloodRiskResponseDto>> GetByCanalAsync(Guid canalId)
    {
<<<<<<< HEAD
        var assessments = await _riskRepo.QueryAsync(q => q
            .WhereEqualTo("CanalId", canalId.ToString())
            .OrderByDescending("AssessmentTimestamp"));
        return await Task.WhenAll(assessments.Select(MapToResponseAsync));
=======
        return await _context.FloodRiskAssessments
            .Where(fra => fra.CanalId == canalId)
            .OrderByDescending(fra => fra.AssessmentTimestamp)
            .Select(fra => MapToResponse(fra))
            .ToListAsync();
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    public async Task<IEnumerable<RiskAnalyticsDto>> GetAnalyticsAsync()
    {
<<<<<<< HEAD
        var canals = await _canalRepo.GetAllAsync();
        var result = new List<RiskAnalyticsDto>();

        foreach (var canal in canals)
        {
            var analytics = await GetAnalyticsByCanalAsync(canal.Id);
            if (analytics != null)
                result.Add(analytics);
        }

        return result;
=======
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
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    public async Task<RiskAnalyticsDto?> GetAnalyticsByCanalAsync(Guid canalId)
    {
<<<<<<< HEAD
        var canal = await _canalRepo.GetByIdAsync(canalId.ToString());
        if (canal == null) return null;

        var totalAlerts = await _alertRepo.CountAsync(q => q.WhereEqualTo("CanalId", canalId.ToString()));
        var activeAlerts = await _alertRepo.CountAsync(q => q.WhereEqualTo("CanalId", canalId.ToString()).WhereEqualTo("Status", "Active"));
        var resolvedAlerts = await _alertRepo.CountAsync(q => q.WhereEqualTo("CanalId", canalId.ToString()).WhereEqualTo("Status", "Resolved"));
        var pendingReports = await _reportRepo.CountAsync(q => q.WhereEqualTo("CanalId", canalId.ToString()).WhereEqualTo("Status", "Pending"));

        var latestReadings = await _readingRepo.QueryAsync(q => q
            .WhereEqualTo("CanalId", canalId.ToString())
            .OrderByDescending("ReadingTimestamp")
            .Limit(10));

        var avgWaterLevel = latestReadings.Any() ? latestReadings.Average(sr => sr.WaterLevel) : 0;

        var latestRisk = (await _riskRepo.QueryAsync(q => q
            .WhereEqualTo("CanalId", canalId.ToString())
            .OrderByDescending("AssessmentTimestamp")
            .Limit(1))).FirstOrDefault();

        var assessments = await _riskRepo.QueryAsync(q => q.WhereEqualTo("CanalId", canalId.ToString()));
        var lastAssessment = assessments.Any() ? assessments.Max(a => a.AssessmentTimestamp) : DateTime.MinValue;
=======
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
>>>>>>> db46004de7d488abfb831db9c6cd307518689719

        return new RiskAnalyticsDto
        {
            CanalId = canal.Id,
            CanalName = canal.Name,
            TotalAlerts = totalAlerts,
            ActiveAlerts = activeAlerts,
            ResolvedAlerts = resolvedAlerts,
            PendingReports = pendingReports,
            AverageWaterLevel = Math.Round(avgWaterLevel, 2),
<<<<<<< HEAD
            CurrentRiskLevel = latestRisk?.RiskLevel ?? "Low",
            LastAssessment = lastAssessment
        };
    }

    private async Task<FloodRiskResponseDto> MapToResponseAsync(FloodRiskAssessment assessment)
    {
        var canal = await _canalRepo.GetByIdAsync(assessment.CanalId.ToString());
=======
            CurrentRiskLevel = latestRisk ?? "Low",
            LastAssessment = canal.FloodRiskAssessments.Any() ? canal.FloodRiskAssessments.Max(fra => fra.AssessmentTimestamp) : DateTime.MinValue
        };
    }

    private static FloodRiskResponseDto MapToResponse(FloodRiskAssessment assessment)
    {
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        return new FloodRiskResponseDto
        {
            Id = assessment.Id,
            CanalId = assessment.CanalId,
<<<<<<< HEAD
            CanalName = canal?.Name ?? string.Empty,
=======
            CanalName = assessment.Canal?.Name ?? string.Empty,
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
            RiskLevel = assessment.RiskLevel,
            RiskScore = assessment.RiskScore,
            PredictionDetails = assessment.PredictionDetails,
            ModelVersion = assessment.ModelVersion,
            AssessmentTimestamp = assessment.AssessmentTimestamp,
            ValidUntil = assessment.ValidUntil
        };
    }
}
