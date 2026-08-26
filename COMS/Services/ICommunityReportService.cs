using COMS.DTOs;
using COMS.Models;

namespace COMS.Services;

public interface ICommunityReportService
{
    Task<CommunityReportResponseDto> CreateAsync(CreateCommunityReportDto dto, Guid reportedByUserId);
    Task<IEnumerable<CommunityReportResponseDto>> GetAllAsync();
    Task<IEnumerable<CommunityReportResponseDto>> GetByCanalAsync(Guid canalId);
<<<<<<< HEAD
    Task<IEnumerable<CommunityReportResponseDto>> GetByUserAsync(Guid userId);
    Task<IEnumerable<CommunityReportResponseDto>> GetCompletedAsync();
    Task<CommunityReportResponseDto?> GetByIdAsync(Guid id);
    Task<CommunityReportResponseDto?> UpdateAsync(Guid id, UpdateCommunityReportDto dto);
    Task<CommunityReportResponseDto?> CompleteAsync(Guid id, CompleteReportDto dto, Guid completedByUserId);
=======
    Task<CommunityReportResponseDto?> GetByIdAsync(Guid id);
    Task<CommunityReportResponseDto?> UpdateAsync(Guid id, UpdateCommunityReportDto dto);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    Task<bool> DeleteAsync(Guid id);
}
