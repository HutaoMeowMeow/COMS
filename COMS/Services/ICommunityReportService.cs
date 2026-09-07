using COMS.DTOs;
using COMS.Models;

namespace COMS.Services;

public interface ICommunityReportService
{
    Task<CommunityReportResponseDto> CreateAsync(CreateCommunityReportDto dto, Guid reportedByUserId);
    Task<IEnumerable<CommunityReportResponseDto>> GetAllAsync();
    Task<IEnumerable<CommunityReportResponseDto>> GetByCanalAsync(Guid canalId);
    Task<CommunityReportResponseDto?> GetByIdAsync(Guid id);
    Task<CommunityReportResponseDto?> UpdateAsync(Guid id, UpdateCommunityReportDto dto);
    Task<bool> DeleteAsync(Guid id);
}
