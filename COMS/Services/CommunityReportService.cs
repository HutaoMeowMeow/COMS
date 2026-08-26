using COMS.Data;
using COMS.DTOs;
using COMS.Models;
<<<<<<< HEAD
using Google.Cloud.Firestore;
=======
using Microsoft.EntityFrameworkCore;
>>>>>>> db46004de7d488abfb831db9c6cd307518689719

namespace COMS.Services;

public class CommunityReportService : ICommunityReportService
{
<<<<<<< HEAD
    private readonly IFirestoreRepository<CommunityReport> _reportRepo;
    private readonly IFirestoreRepository<Canal> _canalRepo;
    private readonly IFirestoreRepository<User> _userRepo;

    public CommunityReportService(
        IFirestoreRepository<CommunityReport> reportRepo,
        IFirestoreRepository<Canal> canalRepo,
        IFirestoreRepository<User> userRepo)
    {
        _reportRepo = reportRepo;
        _canalRepo = canalRepo;
        _userRepo = userRepo;
=======
    private readonly ApplicationDbContext _context;

    public CommunityReportService(ApplicationDbContext context)
    {
        _context = context;
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    public async Task<CommunityReportResponseDto> CreateAsync(CreateCommunityReportDto dto, Guid reportedByUserId)
    {
        var report = new CommunityReport
        {
            Id = Guid.NewGuid(),
            CanalId = dto.CanalId,
            ReportedByUserId = reportedByUserId,
            ReportType = dto.ReportType,
            Title = dto.Title,
            Description = dto.Description,
            ImageUrl = dto.ImageUrl,
            Status = "Pending",
            ReportedAt = DateTime.UtcNow
        };

<<<<<<< HEAD
        await _reportRepo.CreateAsync(report);
=======
        _context.CommunityReports.Add(report);
        await _context.SaveChangesAsync();

>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        return await GetByIdAsync(report.Id) ?? throw new InvalidOperationException("Failed to retrieve created report.");
    }

    public async Task<IEnumerable<CommunityReportResponseDto>> GetAllAsync()
    {
<<<<<<< HEAD
        var reports = await _reportRepo.QueryAsync(q => q.OrderByDescending("ReportedAt"));
        return await Task.WhenAll(reports.Select(MapToResponseAsync));
=======
        return await _context.CommunityReports
            .OrderByDescending(r => r.ReportedAt)
            .Select(r => MapToResponse(r))
            .ToListAsync();
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    public async Task<IEnumerable<CommunityReportResponseDto>> GetByCanalAsync(Guid canalId)
    {
<<<<<<< HEAD
        var reports = await _reportRepo.QueryAsync(q => q
            .WhereEqualTo("CanalId", canalId.ToString())
            .OrderByDescending("ReportedAt"));
        return await Task.WhenAll(reports.Select(MapToResponseAsync));
    }

    public async Task<IEnumerable<CommunityReportResponseDto>> GetByUserAsync(Guid userId)
    {
        var reports = await _reportRepo.QueryAsync(q => q
            .WhereEqualTo("ReportedByUserId", userId.ToString())
            .OrderByDescending("ReportedAt"));
        return await Task.WhenAll(reports.Select(MapToResponseAsync));
    }

    public async Task<IEnumerable<CommunityReportResponseDto>> GetCompletedAsync()
    {
        var reports = await _reportRepo.QueryAsync(q => q
            .WhereEqualTo("Status", "Completed")
            .OrderByDescending("CompletedAt"));
        return await Task.WhenAll(reports.Select(MapToResponseAsync));
=======
        return await _context.CommunityReports
            .Where(r => r.CanalId == canalId)
            .OrderByDescending(r => r.ReportedAt)
            .Select(r => MapToResponse(r))
            .ToListAsync();
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    public async Task<CommunityReportResponseDto?> GetByIdAsync(Guid id)
    {
<<<<<<< HEAD
        var report = await _reportRepo.GetByIdAsync(id.ToString());
        return report == null ? null : await MapToResponseAsync(report);
=======
        var report = await _context.CommunityReports.FindAsync(id);
        return report == null ? null : MapToResponse(report);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    public async Task<CommunityReportResponseDto?> UpdateAsync(Guid id, UpdateCommunityReportDto dto)
    {
<<<<<<< HEAD
        var report = await _reportRepo.GetByIdAsync(id.ToString());
=======
        var report = await _context.CommunityReports.FindAsync(id);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        if (report == null) return null;

        report.Status = dto.Status;
        report.VerifiedBy = dto.VerifiedBy;
        report.ResolutionNotes = dto.ResolutionNotes;

        if (dto.Status == "Verified" && report.VerifiedAt == null)
            report.VerifiedAt = DateTime.UtcNow;
        if (dto.Status == "Resolved" && report.ResolvedAt == null)
            report.ResolvedAt = DateTime.UtcNow;

<<<<<<< HEAD
        await _reportRepo.UpdateAsync(id.ToString(), report);
        return await MapToResponseAsync(report);
    }

    public async Task<CommunityReportResponseDto?> CompleteAsync(Guid id, CompleteReportDto dto, Guid completedByUserId)
    {
        var report = await _reportRepo.GetByIdAsync(id.ToString());
        if (report == null) return null;

        report.Status = "Completed";
        report.CompletionImageUrl = dto.CompletionImageUrl;
        report.CompletionRemarks = dto.CompletionRemarks;
        report.CompletedByUserId = completedByUserId;
        report.CompletedAt = DateTime.UtcNow;

        await _reportRepo.UpdateAsync(id.ToString(), report);
        return await MapToResponseAsync(report);
=======
        await _context.SaveChangesAsync();
        return MapToResponse(report);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
<<<<<<< HEAD
        var report = await _reportRepo.GetByIdAsync(id.ToString());
        if (report == null) return false;

        await _reportRepo.DeleteAsync(id.ToString());
        return true;
    }

    private async Task<CommunityReportResponseDto> MapToResponseAsync(CommunityReport report)
    {
        var canal = await _canalRepo.GetByIdAsync(report.CanalId.ToString());
        var reportedByUser = await _userRepo.GetByIdAsync(report.ReportedByUserId.ToString());
        var completedByUser = report.CompletedByUserId.HasValue 
            ? await _userRepo.GetByIdAsync(report.CompletedByUserId.Value.ToString()) 
            : null;

=======
        var report = await _context.CommunityReports.FindAsync(id);
        if (report == null) return false;

        _context.CommunityReports.Remove(report);
        await _context.SaveChangesAsync();
        return true;
    }

    private static CommunityReportResponseDto MapToResponse(CommunityReport report)
    {
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        return new CommunityReportResponseDto
        {
            Id = report.Id,
            CanalId = report.CanalId,
<<<<<<< HEAD
            CanalName = canal?.Name ?? string.Empty,
            ReportedByUserId = report.ReportedByUserId,
            ReportedByUserName = $"{reportedByUser?.FirstName} {reportedByUser?.LastName}".Trim(),
=======
            CanalName = report.Canal?.Name ?? string.Empty,
            ReportedByUserId = report.ReportedByUserId,
            ReportedByUserName = $"{report.ReportedByUser?.FirstName} {report.ReportedByUser?.LastName}".Trim(),
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
            ReportType = report.ReportType,
            Title = report.Title,
            Description = report.Description,
            ImageUrl = report.ImageUrl,
            Status = report.Status,
            VerifiedBy = report.VerifiedBy,
            ReportedAt = report.ReportedAt,
            VerifiedAt = report.VerifiedAt,
            ResolvedAt = report.ResolvedAt,
<<<<<<< HEAD
            ResolutionNotes = report.ResolutionNotes,
            CompletionImageUrl = report.CompletionImageUrl,
            CompletionRemarks = report.CompletionRemarks,
            CompletedByUserId = report.CompletedByUserId,
            CompletedByUserName = completedByUser != null ? $"{completedByUser.FirstName} {completedByUser.LastName}".Trim() : null,
            CompletedAt = report.CompletedAt
=======
            ResolutionNotes = report.ResolutionNotes
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        };
    }
}
