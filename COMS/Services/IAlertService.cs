using COMS.Data;
using COMS.DTOs;
using COMS.Hubs;
using COMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;

namespace COMS.Services;

public interface IAlertService
{
    Task<AlertResponseDto> CreateAsync(CreateAlertDto dto);
    Task<IEnumerable<AlertResponseDto>> GetAllAsync();
    Task<AlertResponseDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<AlertResponseDto>> GetByCanalAsync(Guid canalId);
    Task<IEnumerable<AlertResponseDto>> GetActiveAsync();
    Task<AlertResponseDto?> UpdateAsync(Guid id, UpdateAlertDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<AlertResponseDto> CreateAlertFromReadingAsync(SensorReading reading, Canal canal);
}
