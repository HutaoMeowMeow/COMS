using COMS.DTOs;

namespace COMS.Services;

public interface ICanalService
{
    Task<CanalResponseDto> CreateAsync(CreateCanalDto dto);
    Task<IEnumerable<CanalResponseDto>> GetAllAsync();
    Task<CanalResponseDto?> GetByIdAsync(Guid id);
    Task<CanalResponseDto?> UpdateAsync(Guid id, UpdateCanalDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<IEnumerable<CanalStatusDto>> GetStatusAsync();
}
