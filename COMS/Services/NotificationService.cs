using COMS.Data;
using COMS.DTOs;
using COMS.Models;
<<<<<<< HEAD
using Google.Cloud.Firestore;
=======
using Microsoft.EntityFrameworkCore;
>>>>>>> db46004de7d488abfb831db9c6cd307518689719

namespace COMS.Services;

public class NotificationService : INotificationService
{
<<<<<<< HEAD
    private readonly IFirestoreRepository<Notification> _notificationRepo;

    public NotificationService(IFirestoreRepository<Notification> notificationRepo)
    {
        _notificationRepo = notificationRepo;
=======
    private readonly ApplicationDbContext _context;

    public NotificationService(ApplicationDbContext context)
    {
        _context = context;
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
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

<<<<<<< HEAD
        await _notificationRepo.CreateAsync(notification);
=======
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        return MapToResponse(notification);
    }

    public async Task<IEnumerable<NotificationResponseDto>> GetByUserAsync(Guid userId)
    {
<<<<<<< HEAD
        var notifications = await _notificationRepo.QueryAsync(q => q
            .WhereEqualTo("UserId", userId.ToString())
            .OrderByDescending("CreatedAt"));
        return notifications.Select(MapToResponse);
=======
        return await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => MapToResponse(n))
            .ToListAsync();
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    public async Task<NotificationResponseDto?> GetByIdAsync(Guid id)
    {
<<<<<<< HEAD
        var notification = await _notificationRepo.GetByIdAsync(id.ToString());
=======
        var notification = await _context.Notifications.FindAsync(id);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        return notification == null ? null : MapToResponse(notification);
    }

    public async Task<int> GetUnreadCountAsync(Guid userId)
    {
<<<<<<< HEAD
        return await _notificationRepo.CountAsync(q => q
            .WhereEqualTo("UserId", userId.ToString())
            .WhereEqualTo("Status", "Unread"));
=======
        return await _context.Notifications.CountAsync(n => n.UserId == userId && n.Status == "Unread");
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    }

    public async Task<bool> MarkAsReadAsync(Guid id)
    {
<<<<<<< HEAD
        var notification = await _notificationRepo.GetByIdAsync(id.ToString());
=======
        var notification = await _context.Notifications.FindAsync(id);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        if (notification == null) return false;

        notification.Status = "Read";
        notification.ReadAt = DateTime.UtcNow;
<<<<<<< HEAD
        await _notificationRepo.UpdateAsync(id.ToString(), notification);
=======
        await _context.SaveChangesAsync();
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
<<<<<<< HEAD
        var notification = await _notificationRepo.GetByIdAsync(id.ToString());
        if (notification == null) return false;

        await _notificationRepo.DeleteAsync(id.ToString());
=======
        var notification = await _context.Notifications.FindAsync(id);
        if (notification == null) return false;

        _context.Notifications.Remove(notification);
        await _context.SaveChangesAsync();
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
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
