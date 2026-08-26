using COMS.Data;
using COMS.DTOs;
using COMS.Models;
using Google.Cloud.Firestore;

namespace COMS.Services;

public interface IAnnouncementService
{
    Task<AnnouncementResponseDto> CreateAsync(CreateAnnouncementDto dto, Guid postedByUserId, string postedByRole);
    Task<IEnumerable<AnnouncementResponseDto>> GetAllAsync();
    Task<AnnouncementResponseDto?> GetByIdAsync(Guid id);
    Task<AnnouncementResponseDto?> UpdateAsync(Guid id, UpdateAnnouncementDto dto);
    Task<bool> DeleteAsync(Guid id);
}

public class AnnouncementService : IAnnouncementService
{
    private readonly IFirestoreRepository<Announcement> _announcementRepo;
    private readonly IFirestoreRepository<User> _userRepo;

    public AnnouncementService(
        IFirestoreRepository<Announcement> announcementRepo,
        IFirestoreRepository<User> userRepo)
    {
        _announcementRepo = announcementRepo;
        _userRepo = userRepo;
    }

    public async Task<AnnouncementResponseDto> CreateAsync(CreateAnnouncementDto dto, Guid postedByUserId, string postedByRole)
    {
        var user = await _userRepo.GetByIdAsync(postedByUserId.ToString());
        
        var announcement = new Announcement
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Content = dto.Content,
            ImageUrl = dto.ImageUrl,
            PostedByUserId = postedByUserId,
            PostedByRole = postedByRole,
            Barangay = user?.Barangay,
            Municipality = user?.Municipality,
            CreatedAt = DateTime.UtcNow
        };

        await _announcementRepo.CreateAsync(announcement);
        return await GetByIdAsync(announcement.Id) ?? throw new InvalidOperationException("Failed to retrieve created announcement.");
    }

    public async Task<IEnumerable<AnnouncementResponseDto>> GetAllAsync()
    {
        var announcements = await _announcementRepo.QueryAsync(q => q.OrderByDescending("CreatedAt"));
        return await Task.WhenAll(announcements.Select(MapToResponseAsync));
    }

    public async Task<AnnouncementResponseDto?> GetByIdAsync(Guid id)
    {
        var announcement = await _announcementRepo.GetByIdAsync(id.ToString());
        return announcement == null ? null : await MapToResponseAsync(announcement);
    }

    public async Task<AnnouncementResponseDto?> UpdateAsync(Guid id, UpdateAnnouncementDto dto)
    {
        var announcement = await _announcementRepo.GetByIdAsync(id.ToString());
        if (announcement == null) return null;

        announcement.Title = dto.Title;
        announcement.Content = dto.Content;
        announcement.ImageUrl = dto.ImageUrl;
        announcement.UpdatedAt = DateTime.UtcNow;

        await _announcementRepo.UpdateAsync(id.ToString(), announcement);
        return await MapToResponseAsync(announcement);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var announcement = await _announcementRepo.GetByIdAsync(id.ToString());
        if (announcement == null) return false;

        await _announcementRepo.DeleteAsync(id.ToString());
        return true;
    }

    private async Task<AnnouncementResponseDto> MapToResponseAsync(Announcement announcement)
    {
        var user = await _userRepo.GetByIdAsync(announcement.PostedByUserId.ToString());

        return new AnnouncementResponseDto
        {
            Id = announcement.Id,
            Title = announcement.Title,
            Content = announcement.Content,
            ImageUrl = announcement.ImageUrl,
            PostedByUserId = announcement.PostedByUserId,
            PostedByUserName = user != null ? $"{user.FirstName} {user.LastName}".Trim() : string.Empty,
            PostedByRole = announcement.PostedByRole,
            Barangay = announcement.Barangay,
            Municipality = announcement.Municipality,
            CreatedAt = announcement.CreatedAt,
            UpdatedAt = announcement.UpdatedAt
        };
    }
}
