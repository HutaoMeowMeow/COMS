using COMS.Data;
using COMS.DTOs;
using COMS.Models;
<<<<<<< HEAD
using Google.Cloud.Firestore;
=======
using Microsoft.EntityFrameworkCore;
>>>>>>> db46004de7d488abfb831db9c6cd307518689719

namespace COMS.Services;

public class CanalService : ICanalService
{
<<<<<<< HEAD
    private readonly IFirestoreRepository<Canal> _canalRepo;
    private readonly IFirestoreRepository<Sensor> _sensorRepo;
    private readonly IFirestoreRepository<SensorReading> _readingRepo;
    private readonly IFirestoreRepository<FloodRiskAssessment> _riskRepo;

    public CanalService(
        IFirestoreRepository<Canal> canalRepo,
        IFirestoreRepository<Sensor> sensorRepo,
        IFirestoreRepository<SensorReading> readingRepo,
        IFirestoreRepository<FloodRiskAssessment> riskRepo)
    {
        _canalRepo = canalRepo;
        _sensorRepo = sensorRepo;
        _readingRepo = readingRepo;
        _riskRepo = riskRepo;
=======
    private readonly ApplicationDbContext _context;

    public CanalService(ApplicationDbContext context)
    {
        _context = context;
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
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

<<<<<<< HEAD
        await _canalRepo.CreateAsync(canal);
=======
        _context.Canals.Add(canal);
        await _context.SaveChangesAsync();

>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        return MapToResponse(canal);
    }

    public async Task<IEnumerable<CanalResponseDto>> GetAllAsync()
    {
<<<<<<< HEAD
        var canals = await _canalRepo.QueryAsync(q => q.OrderByDescending("CreatedAt"));
        var result = new List<CanalResponseDto>();
        
        foreach (var canal in canals)
        {
            var dto = await BuildCanalResponseAsync(canal);
            result.Add(dto);
        }
        
        return result;
=======
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
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    public async Task<CanalResponseDto?> GetByIdAsync(Guid id)
    {
<<<<<<< HEAD
        var canal = await _canalRepo.GetByIdAsync(id.ToString());
        if (canal == null) return null;
        return await BuildCanalResponseAsync(canal);
    }

    public async Task<CanalResponseDto?> UpdateAsync(Guid id, UpdateCanalDto dto)
    {
        var canal = await _canalRepo.GetByIdAsync(id.ToString());
        if (canal == null) return null;

        canal.Name = dto.Name;
        canal.Location = dto.Location;
        canal.Barangay = dto.Barangay;
        canal.Municipality = dto.Municipality;
        canal.Province = dto.Province;
        canal.LengthMeters = dto.LengthMeters;
        canal.WidthMeters = dto.WidthMeters;
        canal.DepthMeters = dto.DepthMeters;
        canal.NormalWaterLevel = dto.NormalWaterLevel;
        canal.WarningWaterLevel = dto.WarningWaterLevel;
        canal.CriticalWaterLevel = dto.CriticalWaterLevel;
        canal.Status = dto.Status;
        canal.UpdatedAt = DateTime.UtcNow;

        await _canalRepo.UpdateAsync(id.ToString(), canal);
        return await BuildCanalResponseAsync(canal);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var canal = await _canalRepo.GetByIdAsync(id.ToString());
        if (canal == null) return false;

        await _canalRepo.DeleteAsync(id.ToString());
        return true;
    }

    public async Task<IEnumerable<CanalStatusDto>> GetStatusAsync()
    {
        var canals = await _canalRepo.QueryAsync(q => q.OrderBy("Name"));
        var result = new List<CanalStatusDto>();

        foreach (var canal in canals)
        {
            var latestReading = (await _readingRepo.QueryAsync(q => q
                .WhereEqualTo("CanalId", canal.Id.ToString())
                .OrderByDescending("ReadingTimestamp")
                .Limit(1))).FirstOrDefault();

            var latestRisk = (await _riskRepo.QueryAsync(q => q
                .WhereEqualTo("CanalId", canal.Id.ToString())
                .OrderByDescending("AssessmentTimestamp")
                .Limit(1))).FirstOrDefault();

            result.Add(new CanalStatusDto
            {
                CanalId = canal.Id,
                CanalName = canal.Name,
                Status = canal.Status,
                CurrentWaterLevel = latestReading?.WaterLevel,
                CurrentRiskLevel = latestRisk?.RiskLevel,
                LastReadingAt = latestReading?.ReadingTimestamp
            });
        }

        return result;
    }

    private async Task<CanalResponseDto> BuildCanalResponseAsync(Canal canal)
    {
        var sensorCount = (await _sensorRepo.CountAsync(q => q.WhereEqualTo("CanalId", canal.Id.ToString())));
=======
        var canal = await _context.Canals.FindAsync(id);
        if (canal == null) return null;
>>>>>>> db46004de7d488abfb831db9c6cd307518689719

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
<<<<<<< HEAD
            SensorCount = sensorCount,
=======
            SensorCount = canal.Sensors.Count,
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
            CreatedAt = canal.CreatedAt
        };
    }

<<<<<<< HEAD
=======
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

>>>>>>> db46004de7d488abfb831db9c6cd307518689719
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
<<<<<<< HEAD
            SensorCount = 0,
=======
            SensorCount = canal.Sensors.Count,
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
            CreatedAt = canal.CreatedAt
        };
    }
}
