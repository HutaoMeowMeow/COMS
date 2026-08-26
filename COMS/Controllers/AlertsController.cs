using COMS.DTOs;
using COMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COMS.Controllers;

[ApiController]
[Route("api/[controller]")]
<<<<<<< HEAD
[Authorize(Roles = "Admin,LGU,Barangay,Maintenance")]
=======
[Authorize(Roles = "Admin,LGU,Barangay,Maintenance,Resident")]
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
public class AlertsController : ControllerBase
{
    private readonly IAlertService _alertService;

    public AlertsController(IAlertService alertService)
    {
        _alertService = alertService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var alerts = await _alertService.GetAllAsync();
        return Ok(alerts);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var alerts = await _alertService.GetActiveAsync();
        return Ok(alerts);
    }

    [HttpGet("canal/{canalId}")]
    public async Task<IActionResult> GetByCanal(Guid canalId)
    {
        var alerts = await _alertService.GetByCanalAsync(canalId);
        return Ok(alerts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var alert = await _alertService.GetByIdAsync(id);
        if (alert == null)
            return NotFound();
        return Ok(alert);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAlertDto dto)
    {
        var alert = await _alertService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = alert.Id }, alert);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAlertDto dto)
    {
        var alert = await _alertService.UpdateAsync(id, dto);
        if (alert == null)
            return NotFound();
        return Ok(alert);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _alertService.DeleteAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
