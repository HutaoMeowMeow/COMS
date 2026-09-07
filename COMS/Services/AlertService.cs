using COMS.Data;
using COMS.DTOs;
using COMS.Hubs;
using COMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;

namespace COMS.Services;

public class AlertService : IAlertService
{
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<MonitoringHub> _hubContext;
    private readonly INotificationService _notificationService;

    public AlertService(ApplicationDbContext context, IHubContext<MonitoringHub> hubContext, INotificationService notificationService)
    {
        _context = context;
        _hubContext = hubContext;
        _notificationService = notificationService;
    }

    public async Task<AlertResponseDto> CreateAsync(CreateAlertDto dto)
    {
        var alert = new ObstructionAlert
        {
            Id = Guid.NewGuid(),
            CanalId = dto.CanalId,
            AlertType = dto.AlertType,
            Severity = dto.Severity,
            Title = dto.Title,
            Description = dto.Description,
            Status = "Active",
            ObstructionType = dto.ObstructionType,
            WaterLevelAtAlert = dto.WaterLevelAtAlert,
            TriggeredAt = DateTime.UtcNow
        };

        _context.ObstructionAlerts.Add(alert);
        await _context.SaveChangesAsync();

        var response = await GetByIdAsync(alert.Id) ?? throw new InvalidOperationException("Failed to retrieve created alert.");
        await NotifyStakeholdersAsync(alert);
        return response;
    }

    public async Task<AlertResponseDto> CreateAlertFromReadingAsync(SensorReading reading, Canal canal)
    {
        var severity = reading.ReadingStatus == "Critical" ? "High" : "Medium";
        var alertType = reading.ReadingStatus == "Critical" ? "Critical Obstruction" : "Warning Obstruction";

        var alert = new ObstructionAlert
        {
            Id = Guid.NewGuid(),
            CanalId = canal.Id,
            AlertType = alertType,
            Severity = severity,
            Title = $"{canal.Name} - {alertType}",
            Description = $"Water level detected at {reading.WaterLevel:F2}m (Status: {reading.ReadingStatus}). Immediate attention required.",
            Status = "Active",
            ObstructionType = reading.DebrisLevel.HasValue && reading.DebrisLevel > 0 ? "Debris Accumulation" : "Water Level Rise",
            WaterLevelAtAlert = reading.WaterLevel,
            TriggeredByReadingId = reading.Id,
            TriggeredAt = DateTime.UtcNow
        };

        _context.ObstructionAlerts.Add(alert);
        await _context.SaveChangesAsync();

        var response = await GetByIdAsync(alert.Id) ?? throw new InvalidOperationException("Failed to retrieve created alert.");
        await NotifyStakeholdersAsync(alert);
        return response;
    }

    public async Task<IEnumerable<AlertResponseDto>> GetAllAsync()
    {
        return await _context.ObstructionAlerts
            .OrderByDescending(a => a.TriggeredAt)
            .Select(a => MapToResponse(a))
            .ToListAsync();
    }

    public async Task<AlertResponseDto?> GetByIdAsync(Guid id)
    {
        var alert = await _context.ObstructionAlerts.FindAsync(id);
        return alert == null ? null : MapToResponse(alert);
    }

    public async Task<IEnumerable<AlertResponseDto>> GetByCanalAsync(Guid canalId)
    {
        return await _context.ObstructionAlerts
            .Where(a => a.CanalId == canalId)
            .OrderByDescending(a => a.TriggeredAt)
            .Select(a => MapToResponse(a))
            .ToListAsync();
    }

    public async Task<IEnumerable<AlertResponseDto>> GetActiveAsync()
    {
        return await _context.ObstructionAlerts
            .Where(a => a.Status == "Active")
            .OrderByDescending(a => a.TriggeredAt)
            .Select(a => MapToResponse(a))
            .ToListAsync();
    }

    public async Task<AlertResponseDto?> UpdateAsync(Guid id, UpdateAlertDto dto)
    {
        var alert = await _context.ObstructionAlerts.FindAsync(id);
        if (alert == null) return null;

        alert.Status = dto.Status;
        if (dto.Status == "Resolved" && alert.ResolvedAt == null)
        {
            alert.ResolvedAt = DateTime.UtcNow;
        }
        alert.ResolutionNotes = dto.ResolutionNotes;
        await _context.SaveChangesAsync();

        return MapToResponse(alert);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var alert = await _context.ObstructionAlerts.FindAsync(id);
        if (alert == null) return false;

        _context.ObstructionAlerts.Remove(alert);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task NotifyStakeholdersAsync(ObstructionAlert alert)
    {
        var users = await _context.Users
            .Where(u => u.IsActive && (u.Role == "Admin" || u.Role == "LGU" || u.Role == "Barangay" || u.Role == "Maintenance"))
            .ToListAsync();

        var canal = await _context.Canals.FindAsync(alert.CanalId);
        var canalName = canal?.Name ?? "Unknown Canal";

        foreach (var user in users)
        {
            await _notificationService.CreateAsync(new CreateNotificationDto
            {
                UserId = user.Id,
                NotificationType = "Alert",
                Title = alert.Title,
                Message = alert.Description,
                Data = $"{{\"alertId\":\"{alert.Id}\",\"canalId\":\"{alert.CanalId}\",\"severity\":\"{alert.Severity}\"}}"
            });
        }

        await _hubContext.Clients.Group($"canal-{alert.CanalId}")
            .SendAsync("NewAlert", new
            {
                alert.Id,
                alert.CanalId,
                CanalName = canalName,
                alert.AlertType,
                alert.Severity,
                alert.Title,
                alert.Description,
                alert.Status,
                alert.TriggeredAt
            });
    }

    private static AlertResponseDto MapToResponse(ObstructionAlert alert)
    {
        return new AlertResponseDto
        {
            Id = alert.Id,
            CanalId = alert.CanalId,
            CanalName = alert.Canal?.Name ?? string.Empty,
            AlertType = alert.AlertType,
            Severity = alert.Severity,
            Title = alert.Title,
            Description = alert.Description,
            Status = alert.Status,
            ObstructionType = alert.ObstructionType,
            WaterLevelAtAlert = alert.WaterLevelAtAlert,
            TriggeredAt = alert.TriggeredAt,
            ResolvedAt = alert.ResolvedAt,
            ResolutionNotes = alert.ResolutionNotes
        };
    }
}
