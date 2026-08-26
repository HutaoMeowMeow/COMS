using COMS.Data;
using COMS.DTOs;
using COMS.Hubs;
using COMS.Models;
<<<<<<< HEAD
using Google.Cloud.Firestore;
=======
using Microsoft.EntityFrameworkCore;
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
using Microsoft.AspNetCore.SignalR;

namespace COMS.Services;

public class AlertService : IAlertService
{
<<<<<<< HEAD
    private readonly IFirestoreRepository<ObstructionAlert> _alertRepo;
    private readonly IFirestoreRepository<User> _userRepo;
    private readonly IFirestoreRepository<Canal> _canalRepo;
    private readonly IFirestoreRepository<Notification> _notificationRepo;
    private readonly IHubContext<MonitoringHub> _hubContext;
    private readonly INotificationService _notificationService;

    public AlertService(
        IFirestoreRepository<ObstructionAlert> alertRepo,
        IFirestoreRepository<User> userRepo,
        IFirestoreRepository<Canal> canalRepo,
        IFirestoreRepository<Notification> notificationRepo,
        IHubContext<MonitoringHub> hubContext,
        INotificationService notificationService)
    {
        _alertRepo = alertRepo;
        _userRepo = userRepo;
        _canalRepo = canalRepo;
        _notificationRepo = notificationRepo;
=======
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<MonitoringHub> _hubContext;
    private readonly INotificationService _notificationService;

    public AlertService(ApplicationDbContext context, IHubContext<MonitoringHub> hubContext, INotificationService notificationService)
    {
        _context = context;
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
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

<<<<<<< HEAD
        await _alertRepo.CreateAsync(alert);
        var response = await MapToResponseAsync(alert) ?? throw new InvalidOperationException("Failed to retrieve created alert.");
=======
        _context.ObstructionAlerts.Add(alert);
        await _context.SaveChangesAsync();

        var response = await GetByIdAsync(alert.Id) ?? throw new InvalidOperationException("Failed to retrieve created alert.");
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
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

<<<<<<< HEAD
        await _alertRepo.CreateAsync(alert);
        var response = await MapToResponseAsync(alert) ?? throw new InvalidOperationException("Failed to retrieve created alert.");
=======
        _context.ObstructionAlerts.Add(alert);
        await _context.SaveChangesAsync();

        var response = await GetByIdAsync(alert.Id) ?? throw new InvalidOperationException("Failed to retrieve created alert.");
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        await NotifyStakeholdersAsync(alert);
        return response;
    }

    public async Task<IEnumerable<AlertResponseDto>> GetAllAsync()
    {
<<<<<<< HEAD
        var alerts = await _alertRepo.QueryAsync(q => q.OrderByDescending("TriggeredAt"));
        return (await Task.WhenAll(alerts.Select(MapToResponseAsync))).Where(x => x != null).Cast<AlertResponseDto>();
=======
        return await _context.ObstructionAlerts
            .OrderByDescending(a => a.TriggeredAt)
            .Select(a => MapToResponse(a))
            .ToListAsync();
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    public async Task<AlertResponseDto?> GetByIdAsync(Guid id)
    {
<<<<<<< HEAD
        var alert = await _alertRepo.GetByIdAsync(id.ToString());
        return alert == null ? null : await MapToResponseAsync(alert);
=======
        var alert = await _context.ObstructionAlerts.FindAsync(id);
        return alert == null ? null : MapToResponse(alert);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    public async Task<IEnumerable<AlertResponseDto>> GetByCanalAsync(Guid canalId)
    {
<<<<<<< HEAD
        var alerts = await _alertRepo.QueryAsync(q => q
            .WhereEqualTo("CanalId", canalId.ToString())
            .OrderByDescending("TriggeredAt"));
        return (await Task.WhenAll(alerts.Select(MapToResponseAsync))).Where(x => x != null).Cast<AlertResponseDto>();
=======
        return await _context.ObstructionAlerts
            .Where(a => a.CanalId == canalId)
            .OrderByDescending(a => a.TriggeredAt)
            .Select(a => MapToResponse(a))
            .ToListAsync();
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    public async Task<IEnumerable<AlertResponseDto>> GetActiveAsync()
    {
<<<<<<< HEAD
        var alerts = await _alertRepo.QueryAsync(q => q
            .WhereEqualTo("Status", "Active")
            .OrderByDescending("TriggeredAt"));
        return (await Task.WhenAll(alerts.Select(MapToResponseAsync))).Where(x => x != null).Cast<AlertResponseDto>();
=======
        return await _context.ObstructionAlerts
            .Where(a => a.Status == "Active")
            .OrderByDescending(a => a.TriggeredAt)
            .Select(a => MapToResponse(a))
            .ToListAsync();
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    public async Task<AlertResponseDto?> UpdateAsync(Guid id, UpdateAlertDto dto)
    {
<<<<<<< HEAD
        var alert = await _alertRepo.GetByIdAsync(id.ToString());
=======
        var alert = await _context.ObstructionAlerts.FindAsync(id);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        if (alert == null) return null;

        alert.Status = dto.Status;
        if (dto.Status == "Resolved" && alert.ResolvedAt == null)
        {
            alert.ResolvedAt = DateTime.UtcNow;
        }
        alert.ResolutionNotes = dto.ResolutionNotes;
<<<<<<< HEAD
        await _alertRepo.UpdateAsync(id.ToString(), alert);

        return await MapToResponseAsync(alert);
=======
        await _context.SaveChangesAsync();

        return MapToResponse(alert);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
<<<<<<< HEAD
        var alert = await _alertRepo.GetByIdAsync(id.ToString());
        if (alert == null) return false;

        await _alertRepo.DeleteAsync(id.ToString());
=======
        var alert = await _context.ObstructionAlerts.FindAsync(id);
        if (alert == null) return false;

        _context.ObstructionAlerts.Remove(alert);
        await _context.SaveChangesAsync();
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        return true;
    }

    private async Task NotifyStakeholdersAsync(ObstructionAlert alert)
    {
<<<<<<< HEAD
        var users = await _userRepo.QueryAsync(q => q
            .WhereEqualTo("IsActive", true)
            .WhereIn("Role", new[] { "Admin", "LGU", "Barangay", "Maintenance" }));

        var canal = await _canalRepo.GetByIdAsync(alert.CanalId.ToString());
=======
        var users = await _context.Users
            .Where(u => u.IsActive && (u.Role == "Admin" || u.Role == "LGU" || u.Role == "Barangay" || u.Role == "Maintenance"))
            .ToListAsync();

        var canal = await _context.Canals.FindAsync(alert.CanalId);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
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

<<<<<<< HEAD
    private async Task<AlertResponseDto?> MapToResponseAsync(ObstructionAlert alert)
    {
        var canal = await _canalRepo.GetByIdAsync(alert.CanalId.ToString());
=======
    private static AlertResponseDto MapToResponse(ObstructionAlert alert)
    {
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        return new AlertResponseDto
        {
            Id = alert.Id,
            CanalId = alert.CanalId,
<<<<<<< HEAD
            CanalName = canal?.Name ?? string.Empty,
=======
            CanalName = alert.Canal?.Name ?? string.Empty,
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
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
