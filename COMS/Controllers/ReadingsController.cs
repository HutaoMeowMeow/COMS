using COMS.DTOs;
using COMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COMS.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class ReadingsController : ControllerBase
{
    private readonly ISensorService _sensorService;

    public ReadingsController(ISensorService sensorService)
    {
        _sensorService = sensorService;
    }

    [HttpPost("ingest")]
    public async Task<IActionResult> IngestReading([FromBody] SensorReadingIngestionDto dto)
    {
        try
        {
            var reading = await _sensorService.IngestReadingAsync(dto);
            return Ok(new { Message = "Reading ingested successfully", Reading = reading });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("sensor/{sensorId}")]
    public async Task<IActionResult> GetBySensor(Guid sensorId, [FromQuery] int limit = 100)
    {
        var readings = await _sensorService.GetReadingsBySensorAsync(sensorId, limit);
        return Ok(readings);
    }

    [HttpGet("canal/{canalId}")]
    public async Task<IActionResult> GetByCanal(Guid canalId, [FromQuery] int limit = 100)
    {
        var readings = await _sensorService.GetReadingsByCanalAsync(canalId, limit);
        return Ok(readings);
    }
}
