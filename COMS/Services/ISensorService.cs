using COMS.Data;
using COMS.DTOs;
using COMS.Models;
<<<<<<< HEAD
=======
using Microsoft.EntityFrameworkCore;
>>>>>>> db46004de7d488abfb831db9c6cd307518689719

namespace COMS.Services;

public interface ISensorService
{
    Task<SensorResponseDto> CreateAsync(CreateSensorDto dto);
    Task<IEnumerable<SensorResponseDto>> GetAllAsync();
    Task<IEnumerable<SensorResponseDto>> GetByCanalAsync(Guid canalId);
    Task<SensorResponseDto?> GetByIdAsync(Guid id);
    Task<SensorResponseDto?> UpdateAsync(Guid id, UpdateSensorDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<SensorReadingResponseDto> IngestReadingAsync(SensorReadingIngestionDto dto);
    Task<IEnumerable<SensorReadingResponseDto>> GetReadingsBySensorAsync(Guid sensorId, int limit = 100);
<<<<<<< HEAD
=======
    Task<IEnumerable<SensorReadingResponseDto>> GetReadingsByCanalAsync(Guid canalId, int limit = 100);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
}
