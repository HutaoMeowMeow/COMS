using COMS.Data;
using COMS.DTOs;
using COMS.Hubs;
using COMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;

namespace COMS.Services;

public class SensorService : ISensorService
{
    private readonly ApplicationDbContext _context;
    private readonly IAlertService _alertService;
    private readonly IHubContext<MonitoringHub> _hubContext;
    private readonly ILogger<SensorService> _logger;

    public SensorService(ApplicationDbContext context, IAlertService alertService, IHubContext<MonitoringHub> hubContext, ILogger<SensorService> logger)
    {
        _context = context;
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

        _context.Sensors.Add(sensor);
        await _context.SaveChangesAsync();

        return MapToResponse(sensor);
    }

    public async Task<IEnumerable<SensorResponseDto>> GetAllAsync()
    {
        return await _context.Sensors
            .OrderByDescending(s => s.CreatedAt)
            .Select(s => MapToResponse(s))
            .ToListAsync();
    }

    public async Task<IEnumerable<SensorResponseDto>> GetByCanalAsync(Guid canalId)
    {
        return await _context.Sensors
            .Where(s => s.CanalId == canalId)
            .OrderBy(s => s.SensorCode)
            .Select(s => MapToResponse(s))
            .ToListAsync();
    }

    public async Task<SensorResponseDto?> GetByIdAsync(Guid id)
    {
        var sensor = await _context.Sensors.FindAsync(id);
        return sensor == null ? null : MapToResponse(sensor);
    }

    public async Task<SensorResponseDto?> UpdateAsync(Guid id, UpdateSensorDto dto)
    {
        var sensor = await _context.Sensors.FindAsync(id);
        if (sensor == null) return null;

        sensor.SensorCode = dto.SensorCode;
        sensor.SensorType = dto.SensorType;
        sensor.LocationDescription = dto.LocationDescription;
        sensor.Latitude = dto.Latitude;
        sensor.Longitude = dto.Longitude;
        sensor.CommunicationProtocol = dto.CommunicationProtocol;
        sensor.Status = dto.Status;
        sensor.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return MapToResponse(sensor);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var sensor = await _context.Sensors.FindAsync(id);
        if (sensor == null) return false;

        _context.Sensors.Remove(sensor);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<SensorReadingResponseDto> IngestReadingAsync(SensorReadingIngestionDto dto)
    {
        var sensor = await _context.Sensors
            .FirstOrDefaultAsync(s => s.SensorCode == dto.SensorCode);

        if (sensor == null)
            throw new InvalidOperationException($"Sensor {dto.SensorCode} not found.");

        var canal = await _context.Canals.FindAsync(sensor.CanalId);
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

        _context.SensorReadings.Add(reading);
        sensor.LastReadingAt = reading.ReadingTimestamp;
        await _context.SaveChangesAsync();

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
    }

    private static string DetermineReadingStatus(Canal canal, double waterLevel)
    {
        if (waterLevel >= canal.CriticalWaterLevel)
            return "Critical";
        if (waterLevel >= canal.WarningWaterLevel)
            return "Warning";
        return "Normal";
    }

    private static SensorResponseDto MapToResponse(Sensor sensor)
    {
        return new SensorResponseDto
        {
            Id = sensor.Id,
            SensorCode = sensor.SensorCode,
            SensorType = sensor.SensorType,
            CanalId = sensor.CanalId,
            CanalName = sensor.Canal?.Name ?? string.Empty,
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
