using COMS.Data;
using COMS.DTOs;
using COMS.Hubs;
using COMS.Models;
<<<<<<< HEAD
using Google.Cloud.Firestore;
=======
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
using Microsoft.AspNetCore.SignalR;

namespace COMS.Services;

public class SensorService : ISensorService
{
<<<<<<< HEAD
    private readonly IFirestoreRepository<Sensor> _sensorRepo;
    private readonly IFirestoreRepository<Canal> _canalRepo;
    private readonly IFirestoreRepository<SensorReading> _readingRepo;
=======
    private readonly ApplicationDbContext _context;
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    private readonly IAlertService _alertService;
    private readonly IHubContext<MonitoringHub> _hubContext;
    private readonly ILogger<SensorService> _logger;

<<<<<<< HEAD
    public SensorService(
        IFirestoreRepository<Sensor> sensorRepo,
        IFirestoreRepository<Canal> canalRepo,
        IFirestoreRepository<SensorReading> readingRepo,
        IAlertService alertService,
        IHubContext<MonitoringHub> hubContext,
        ILogger<SensorService> logger)
    {
        _sensorRepo = sensorRepo;
        _canalRepo = canalRepo;
        _readingRepo = readingRepo;
=======
    public SensorService(ApplicationDbContext context, IAlertService alertService, IHubContext<MonitoringHub> hubContext, ILogger<SensorService> logger)
    {
        _context = context;
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        _alertService = alertService;
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task<SensorResponseDto> CreateAsync(CreateSensorDto dto)
    {
        var sensor = new Sensor
        {
            Id = Guid.NewGuid(),
            SensorCode = dto.SensorCode,
            SensorType = dto.SensorType,
            CanalId = dto.CanalId,
            LocationDescription = dto.LocationDescription,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            CommunicationProtocol = dto.CommunicationProtocol,
            Status = "Online",
            CreatedAt = DateTime.UtcNow
        };

<<<<<<< HEAD
        await _sensorRepo.CreateAsync(sensor);
        return await MapToResponseAsync(sensor);
=======
        _context.Sensors.Add(sensor);
        await _context.SaveChangesAsync();

        return MapToResponse(sensor);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    public async Task<IEnumerable<SensorResponseDto>> GetAllAsync()
    {
<<<<<<< HEAD
        var sensors = await _sensorRepo.QueryAsync(q => q.OrderByDescending("CreatedAt"));
        var result = new List<SensorResponseDto>();
        foreach (var sensor in sensors)
        {
            result.Add(await MapToResponseAsync(sensor));
        }
        return result;
=======
        return await _context.Sensors
            .OrderByDescending(s => s.CreatedAt)
            .Select(s => MapToResponse(s))
            .ToListAsync();
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    public async Task<IEnumerable<SensorResponseDto>> GetByCanalAsync(Guid canalId)
    {
<<<<<<< HEAD
        var sensors = await _sensorRepo.QueryAsync(q => q
            .WhereEqualTo("CanalId", canalId.ToString())
            .OrderBy("SensorCode"));
        
        var result = new List<SensorResponseDto>();
        foreach (var sensor in sensors)
        {
            result.Add(await MapToResponseAsync(sensor));
        }
        return result;
=======
        return await _context.Sensors
            .Where(s => s.CanalId == canalId)
            .OrderBy(s => s.SensorCode)
            .Select(s => MapToResponse(s))
            .ToListAsync();
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    public async Task<SensorResponseDto?> GetByIdAsync(Guid id)
    {
<<<<<<< HEAD
        var sensor = await _sensorRepo.GetByIdAsync(id.ToString());
        return sensor == null ? null : await MapToResponseAsync(sensor);
=======
        var sensor = await _context.Sensors.FindAsync(id);
        return sensor == null ? null : MapToResponse(sensor);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    public async Task<SensorResponseDto?> UpdateAsync(Guid id, UpdateSensorDto dto)
    {
<<<<<<< HEAD
        var sensor = await _sensorRepo.GetByIdAsync(id.ToString());
=======
        var sensor = await _context.Sensors.FindAsync(id);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        if (sensor == null) return null;

        sensor.SensorCode = dto.SensorCode;
        sensor.SensorType = dto.SensorType;
        sensor.LocationDescription = dto.LocationDescription;
        sensor.Latitude = dto.Latitude;
        sensor.Longitude = dto.Longitude;
        sensor.CommunicationProtocol = dto.CommunicationProtocol;
        sensor.Status = dto.Status;
        sensor.UpdatedAt = DateTime.UtcNow;

<<<<<<< HEAD
        await _sensorRepo.UpdateAsync(id.ToString(), sensor);
        return await MapToResponseAsync(sensor);
=======
        await _context.SaveChangesAsync();
        return MapToResponse(sensor);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
<<<<<<< HEAD
        var sensor = await _sensorRepo.GetByIdAsync(id.ToString());
        if (sensor == null) return false;

        await _sensorRepo.DeleteAsync(id.ToString());
=======
        var sensor = await _context.Sensors.FindAsync(id);
        if (sensor == null) return false;

        _context.Sensors.Remove(sensor);
        await _context.SaveChangesAsync();
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        return true;
    }

    public async Task<SensorReadingResponseDto> IngestReadingAsync(SensorReadingIngestionDto dto)
    {
<<<<<<< HEAD
        var sensor = (await _sensorRepo.QueryAsync(q => q
            .WhereEqualTo("SensorCode", dto.SensorCode)
            .Limit(1))).FirstOrDefault();
=======
        var sensor = await _context.Sensors
            .FirstOrDefaultAsync(s => s.SensorCode == dto.SensorCode);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719

        if (sensor == null)
            throw new InvalidOperationException($"Sensor {dto.SensorCode} not found.");

<<<<<<< HEAD
        var canal = await _canalRepo.GetByIdAsync(sensor.CanalId.ToString());
=======
        var canal = await _context.Canals.FindAsync(sensor.CanalId);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        if (canal == null)
            throw new InvalidOperationException($"Canal not found for sensor {dto.SensorCode}.");

        var readingStatus = DetermineReadingStatus(canal, dto.WaterLevel);

        var reading = new SensorReading
        {
            Id = Guid.NewGuid(),
            SensorId = sensor.Id,
            CanalId = sensor.CanalId,
            WaterLevel = dto.WaterLevel,
            FlowRate = dto.FlowRate,
            DebrisLevel = dto.DebrisLevel,
            Turbidity = dto.Turbidity,
            Temperature = dto.Temperature,
            ReadingStatus = readingStatus,
            ReadingTimestamp = dto.ReadingTimestamp.ToUniversalTime(),
            ReceivedAt = DateTime.UtcNow
        };

<<<<<<< HEAD
        await _readingRepo.CreateAsync(reading);

        sensor.LastReadingAt = reading.ReadingTimestamp;
        await _sensorRepo.UpdateAsync(sensor.Id.ToString(), sensor);
=======
        _context.SensorReadings.Add(reading);
        sensor.LastReadingAt = reading.ReadingTimestamp;
        await _context.SaveChangesAsync();
>>>>>>> db46004de7d488abfb831db9c6cd307518689719

        if (readingStatus == "Warning" || readingStatus == "Critical")
        {
            await _alertService.CreateAlertFromReadingAsync(reading, canal);
        }

        var responseDto = new SensorReadingResponseDto
        {
            Id = reading.Id,
            SensorId = reading.SensorId,
            SensorCode = sensor.SensorCode,
            CanalId = reading.CanalId,
            CanalName = canal.Name,
            WaterLevel = reading.WaterLevel,
            FlowRate = reading.FlowRate,
            DebrisLevel = reading.DebrisLevel,
            Turbidity = reading.Turbidity,
            Temperature = reading.Temperature,
            ReadingStatus = reading.ReadingStatus,
            ReadingTimestamp = reading.ReadingTimestamp,
            ReceivedAt = reading.ReceivedAt
        };

        await _hubContext.Clients.Group($"canal-{canal.Id}")
            .SendAsync("NewReading", responseDto);

        return responseDto;
    }

    public async Task<IEnumerable<SensorReadingResponseDto>> GetReadingsBySensorAsync(Guid sensorId, int limit = 100)
    {
<<<<<<< HEAD
        var readings = await _readingRepo.QueryAsync(q => q
            .WhereEqualTo("SensorId", sensorId.ToString())
            .OrderByDescending("ReadingTimestamp")
            .Limit(limit));

        var result = new List<SensorReadingResponseDto>();
        foreach (var sr in readings)
        {
            var sensor = await _sensorRepo.GetByIdAsync(sr.SensorId.ToString());
            var canal = sensor != null ? await _canalRepo.GetByIdAsync(sensor.CanalId.ToString()) : null;
            
            result.Add(new SensorReadingResponseDto
            {
                Id = sr.Id,
                SensorId = sr.SensorId,
                SensorCode = sensor?.SensorCode ?? string.Empty,
                CanalId = sr.CanalId,
                CanalName = canal?.Name ?? string.Empty,
=======
        return await _context.SensorReadings
            .Where(sr => sr.SensorId == sensorId)
            .OrderByDescending(sr => sr.ReadingTimestamp)
            .Take(limit)
            .Select(sr => new SensorReadingResponseDto
            {
                Id = sr.Id,
                SensorId = sr.SensorId,
                SensorCode = sr.Sensor!.SensorCode,
                CanalId = sr.CanalId,
                CanalName = sr.Canal!.Name,
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
                WaterLevel = sr.WaterLevel,
                FlowRate = sr.FlowRate,
                DebrisLevel = sr.DebrisLevel,
                Turbidity = sr.Turbidity,
                Temperature = sr.Temperature,
                ReadingStatus = sr.ReadingStatus,
                ReadingTimestamp = sr.ReadingTimestamp,
                ReceivedAt = sr.ReceivedAt
<<<<<<< HEAD
            });
        }
        return result;
=======
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<SensorReadingResponseDto>> GetReadingsByCanalAsync(Guid canalId, int limit = 100)
    {
        return await _context.SensorReadings
            .Where(sr => sr.CanalId == canalId)
            .OrderByDescending(sr => sr.ReadingTimestamp)
            .Take(limit)
            .Select(sr => new SensorReadingResponseDto
            {
                Id = sr.Id,
                SensorId = sr.SensorId,
                SensorCode = sr.Sensor!.SensorCode,
                CanalId = sr.CanalId,
                CanalName = sr.Canal!.Name,
                WaterLevel = sr.WaterLevel,
                FlowRate = sr.FlowRate,
                DebrisLevel = sr.DebrisLevel,
                Turbidity = sr.Turbidity,
                Temperature = sr.Temperature,
                ReadingStatus = sr.ReadingStatus,
                ReadingTimestamp = sr.ReadingTimestamp,
                ReceivedAt = sr.ReceivedAt
            })
            .ToListAsync();
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    private static string DetermineReadingStatus(Canal canal, double waterLevel)
    {
        if (waterLevel >= canal.CriticalWaterLevel)
            return "Critical";
        if (waterLevel >= canal.WarningWaterLevel)
            return "Warning";
        return "Normal";
    }

<<<<<<< HEAD
    private async Task<SensorResponseDto> MapToResponseAsync(Sensor sensor)
    {
        var canal = await _canalRepo.GetByIdAsync(sensor.CanalId.ToString());
=======
    private static SensorResponseDto MapToResponse(Sensor sensor)
    {
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        return new SensorResponseDto
        {
            Id = sensor.Id,
            SensorCode = sensor.SensorCode,
            SensorType = sensor.SensorType,
            CanalId = sensor.CanalId,
<<<<<<< HEAD
            CanalName = canal?.Name ?? string.Empty,
=======
            CanalName = sensor.Canal?.Name ?? string.Empty,
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
            LocationDescription = sensor.LocationDescription,
            Latitude = sensor.Latitude,
            Longitude = sensor.Longitude,
            CommunicationProtocol = sensor.CommunicationProtocol,
            Status = sensor.Status,
            LastReadingAt = sensor.LastReadingAt,
            CreatedAt = sensor.CreatedAt
        };
    }
}
