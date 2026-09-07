using COMS.Data;
using COMS.DTOs;
using COMS.Models;
using Microsoft.EntityFrameworkCore;

namespace COMS.Services;

public class NotificationService : INotificationService
{
    private readonly ApplicationDbContext _context;

    public NotificationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<NotificationResponseDto> CreateAsync(CreateNotificationDto dto)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = dto.UserId,
            NotificationType = dto.NotificationType,
            Title = dto.Title,
            Message = dto.Message,
            Data = dto.Data,
            Status = "Unread",
            CreatedAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        return MapToResponse(notification);
    }

    public async Task<IEnumerable<NotificationResponseDto>> GetByUserAsync(Guid userId)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => MapToResponse(n))
            .ToListAsync();
    }

    public async Task<NotificationResponseDto?> GetByIdAsync(Guid id)
    {
        var notification = await _context.Notifications.FindAsync(id);
        return notification == null ? null : MapToResponse(notification);
    }

    public async Task<int> GetUnreadCountAsync(Guid userId)
    {
        return await _context.Notifications.CountAsync(n => n.UserId == userId && n.Status == "Unread");
    }

    public async Task<bool> MarkAsReadAsync(Guid id)
    {
        var notification = await _context.Notifications.FindAsync(id);
        if (notification == null) return false;

        notification.Status = "Read";
        notification.ReadAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var notification = await _context.Notifications.FindAsync(id);
        if (notification == null) return false;

        _context.Notifications.Remove(notification);
        await _context.SaveChangesAsync();
        return true;
    }

    private static NotificationResponseDto MapToResponse(Notification notification)
    {
        return new NotificationResponseDto
        {
            Id = notification.Id,
            NotificationType = notification.NotificationType,
            Title = notification.Title,
            Message = notification.Message,
            Status = notification.Status,
            CreatedAt = notification.CreatedAt,
            ReadAt = notification.ReadAt
        };
    }
}
