using COMS.DTOs;
using COMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COMS.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,LGU,Barangay,Maintenance")]
public class SensorsController : ControllerBase
{
    private readonly ISensorService _sensorService;

    public SensorsController(ISensorService sensorService)
    {
        _sensorService = sensorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var sensors = await _sensorService.GetAllAsync();
        return Ok(sensors);
    }

    [HttpGet("canal/{canalId}")]
    public async Task<IActionResult> GetByCanal(Guid canalId)
    {
        var sensors = await _sensorService.GetByCanalAsync(canalId);
        return Ok(sensors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var sensor = await _sensorService.GetByIdAsync(id);
        if (sensor == null)
            return NotFound();
        return Ok(sensor);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSensorDto dto)
    {
        var sensor = await _sensorService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = sensor.Id }, sensor);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSensorDto dto)
    {
        var sensor = await _sensorService.UpdateAsync(id, dto);
        if (sensor == null)
            return NotFound();
        return Ok(sensor);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _sensorService.DeleteAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
