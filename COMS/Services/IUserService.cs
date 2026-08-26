using COMS.DTOs;
using COMS.Models;

namespace COMS.Services;

public interface IUserService
{
    Task<UserResponseDto> RegisterAsync(RegisterUserDto dto);
    Task<string?> LoginAsync(LoginUserDto dto);
    Task<UserResponseDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<UserResponseDto>> GetAllAsync();
    Task<UserResponseDto?> UpdateAsync(Guid id, UpdateUserDto dto);
    Task<bool> DeleteAsync(Guid id);
}
