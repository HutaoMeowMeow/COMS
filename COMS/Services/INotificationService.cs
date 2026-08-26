using COMS.Data;
using COMS.DTOs;
using COMS.Models;
<<<<<<< HEAD
=======
using Microsoft.EntityFrameworkCore;
>>>>>>> db46004de7d488abfb831db9c6cd307518689719

namespace COMS.Services;

public interface INotificationService
{
    Task<NotificationResponseDto> CreateAsync(CreateNotificationDto dto);
    Task<IEnumerable<NotificationResponseDto>> GetByUserAsync(Guid userId);
    Task<NotificationResponseDto?> GetByIdAsync(Guid id);
    Task<int> GetUnreadCountAsync(Guid userId);
    Task<bool> MarkAsReadAsync(Guid id);
    Task<bool> DeleteAsync(Guid id);
}
