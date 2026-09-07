using COMS.Data;
using COMS.DTOs;
using COMS.Models;
using Microsoft.EntityFrameworkCore;

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
