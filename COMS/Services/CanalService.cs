using COMS.Data;
using COMS.DTOs;
using COMS.Models;
using Microsoft.EntityFrameworkCore;

namespace COMS.Services;

public class CanalService : ICanalService
{
    private readonly ApplicationDbContext _context;

    public CanalService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CanalResponseDto> CreateAsync(CreateCanalDto dto)
    {
        var canal = new Canal
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Location = dto.Location,
            Barangay = dto.Barangay,
            Municipality = dto.Municipality,
            Province = dto.Province,
            LengthMeters = dto.LengthMeters,
            WidthMeters = dto.WidthMeters,
            DepthMeters = dto.DepthMeters,
            NormalWaterLevel = dto.NormalWaterLevel,
            WarningWaterLevel = dto.WarningWaterLevel,
            CriticalWaterLevel = dto.CriticalWaterLevel,
            CreatedAt = DateTime.UtcNow
        };

        _context.Canals.Add(canal);
        await _context.SaveChangesAsync();

        return MapToResponse(canal);
    }

    public async Task<IEnumerable<CanalResponseDto>> GetAllAsync()
    {
        return await _context.Canals
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new CanalResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Location = c.Location,
                Barangay = c.Barangay,
                Municipality = c.Municipality,
                Province = c.Province,
                Status = c.Status,
                LengthMeters = c.LengthMeters,
                WidthMeters = c.WidthMeters,
                DepthMeters = c.DepthMeters,
                NormalWaterLevel = c.NormalWaterLevel,
                WarningWaterLevel = c.WarningWaterLevel,
                CriticalWaterLevel = c.CriticalWaterLevel,
                SensorCount = c.Sensors.Count,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<CanalResponseDto?> GetByIdAsync(Guid id)
    {
        var canal = await _context.Canals.FindAsync(id);
        if (canal == null) return null;

        return new CanalResponseDto
        {
            Id = canal.Id,
            Name = canal.Name,
            Location = canal.Location,
            Barangay = canal.Barangay,
            Municipality = canal.Municipality,
            Province = canal.Province,
            Status = canal.Status,
            LengthMeters = canal.LengthMeters,
            WidthMeters = canal.WidthMeters,
            DepthMeters = canal.DepthMeters,
            NormalWaterLevel = canal.NormalWaterLevel,
            WarningWaterLevel = canal.WarningWaterLevel,
            CriticalWaterLevel = canal.CriticalWaterLevel,
            SensorCount = canal.Sensors.Count,
            CreatedAt = canal.CreatedAt
        };
    }

    public async Task<CanalResponseDto?> UpdateAsync(Guid id, UpdateCanalDto dto)
    {
        var canal = await _context.Canals.FindAsync(id);
        if (canal == null) return null;

        if (dto.Name != null) canal.Name = dto.Name;
        if (dto.Location != null) canal.Location = dto.Location;
        if (dto.Barangay != null) canal.Barangay = dto.Barangay;
        if (dto.Municipality != null) canal.Municipality = dto.Municipality;
        if (dto.Province != null) canal.Province = dto.Province;
        if (dto.LengthMeters.HasValue) canal.LengthMeters = dto.LengthMeters.Value;
        if (dto.WidthMeters.HasValue) canal.WidthMeters = dto.WidthMeters.Value;
        if (dto.DepthMeters.HasValue) canal.DepthMeters = dto.DepthMeters.Value;
        if (dto.NormalWaterLevel.HasValue) canal.NormalWaterLevel = dto.NormalWaterLevel.Value;
        if (dto.WarningWaterLevel.HasValue) canal.WarningWaterLevel = dto.WarningWaterLevel.Value;
        if (dto.CriticalWaterLevel.HasValue) canal.CriticalWaterLevel = dto.CriticalWaterLevel.Value;
        if (dto.Status != null) canal.Status = dto.Status;

        canal.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return MapToResponse(canal);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var canal = await _context.Canals.FindAsync(id);
        if (canal == null) return false;

        _context.Canals.Remove(canal);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<CanalStatusDto>> GetStatusAsync()
    {
        var now = DateTime.UtcNow;

        return await _context.Canals
            .OrderBy(c => c.Name)
            .Select(c => new CanalStatusDto
            {
                CanalId = c.Id,
                CanalName = c.Name,
                Status = c.Status,
                CurrentWaterLevel = c.SensorReadings
                    .OrderByDescending(sr => sr.ReadingTimestamp)
                    .Select(sr => (double?)sr.WaterLevel)
                    .FirstOrDefault(),
                CurrentRiskLevel = c.FloodRiskAssessments
                    .OrderByDescending(fra => fra.AssessmentTimestamp)
                    .Select(fra => fra.RiskLevel)
                    .FirstOrDefault(),
                LastReadingAt = c.SensorReadings
                    .OrderByDescending(sr => sr.ReadingTimestamp)
                    .Select(sr => (DateTime?)sr.ReadingTimestamp)
                    .FirstOrDefault()
            })
            .ToListAsync();
    }

    private static CanalResponseDto MapToResponse(Canal canal)
    {
        return new CanalResponseDto
        {
            Id = canal.Id,
            Name = canal.Name,
            Location = canal.Location,
            Barangay = canal.Barangay,
            Municipality = canal.Municipality,
            Province = canal.Province,
            Status = canal.Status,
            LengthMeters = canal.LengthMeters,
            WidthMeters = canal.WidthMeters,
            DepthMeters = canal.DepthMeters,
            NormalWaterLevel = canal.NormalWaterLevel,
            WarningWaterLevel = canal.WarningWaterLevel,
            CriticalWaterLevel = canal.CriticalWaterLevel,
            SensorCount = canal.Sensors.Count,
            CreatedAt = canal.CreatedAt
        };
    }
}
