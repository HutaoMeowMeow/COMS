using COMS.Data;
using COMS.DTOs;
using COMS.Models;
using Microsoft.EntityFrameworkCore;

namespace COMS.Services;

public class CommunityReportService : ICommunityReportService
{
    private readonly ApplicationDbContext _context;

    public CommunityReportService(ApplicationDbContext context)
    {
        _context = context;
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

        _context.CommunityReports.Add(report);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(report.Id) ?? throw new InvalidOperationException("Failed to retrieve created report.");
    }

    public async Task<IEnumerable<CommunityReportResponseDto>> GetAllAsync()
    {
        return await _context.CommunityReports
            .OrderByDescending(r => r.ReportedAt)
            .Select(r => MapToResponse(r))
            .ToListAsync();
    }

    public async Task<IEnumerable<CommunityReportResponseDto>> GetByCanalAsync(Guid canalId)
    {
        return await _context.CommunityReports
            .Where(r => r.CanalId == canalId)
            .OrderByDescending(r => r.ReportedAt)
            .Select(r => MapToResponse(r))
            .ToListAsync();
    }

    public async Task<CommunityReportResponseDto?> GetByIdAsync(Guid id)
    {
        var report = await _context.CommunityReports.FindAsync(id);
        return report == null ? null : MapToResponse(report);
    }

    public async Task<CommunityReportResponseDto?> UpdateAsync(Guid id, UpdateCommunityReportDto dto)
    {
        var report = await _context.CommunityReports.FindAsync(id);
        if (report == null) return null;

        report.Status = dto.Status;
        report.VerifiedBy = dto.VerifiedBy;
        report.ResolutionNotes = dto.ResolutionNotes;

        if (dto.Status == "Verified" && report.VerifiedAt == null)
            report.VerifiedAt = DateTime.UtcNow;
        if (dto.Status == "Resolved" && report.ResolvedAt == null)
            report.ResolvedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return MapToResponse(report);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var report = await _context.CommunityReports.FindAsync(id);
        if (report == null) return false;

        _context.CommunityReports.Remove(report);
        await _context.SaveChangesAsync();
        return true;
    }

    private static CommunityReportResponseDto MapToResponse(CommunityReport report)
    {
        return new CommunityReportResponseDto
        {
            Id = report.Id,
            CanalId = report.CanalId,
            CanalName = report.Canal?.Name ?? string.Empty,
            ReportedByUserId = report.ReportedByUserId,
            ReportedByUserName = $"{report.ReportedByUser?.FirstName} {report.ReportedByUser?.LastName}".Trim(),
            ReportType = report.ReportType,
            Title = report.Title,
            Description = report.Description,
            ImageUrl = report.ImageUrl,
            Status = report.Status,
            VerifiedBy = report.VerifiedBy,
            ReportedAt = report.ReportedAt,
            VerifiedAt = report.VerifiedAt,
            ResolvedAt = report.ResolvedAt,
            ResolutionNotes = report.ResolutionNotes
        };
    }
}
