using COMS.DTOs;
using COMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COMS.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly ICommunityReportService _reportService;

    public ReportsController(ICommunityReportService reportService)
    {
        _reportService = reportService;
    }

    [Authorize(Roles = "Admin,LGU,Barangay,Maintenance")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var reports = await _reportService.GetAllAsync();
        return Ok(reports);
    }

    [Authorize(Roles = "Admin,LGU,Barangay,Maintenance")]
    [HttpGet("canal/{canalId}")]
    public async Task<IActionResult> GetByCanal(Guid canalId)
    {
        var reports = await _reportService.GetByCanalAsync(canalId);
        return Ok(reports);
    }

<<<<<<< HEAD
    [Authorize]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyReports()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var reports = await _reportService.GetByUserAsync(Guid.Parse(userId));
        return Ok(reports);
    }

    [Authorize]
    [HttpGet("completed")]
    public async Task<IActionResult> GetCompletedReports()
    {
        var reports = await _reportService.GetCompletedAsync();
        return Ok(reports);
    }

=======
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    [Authorize(Roles = "Admin,LGU,Barangay,Maintenance")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var report = await _reportService.GetByIdAsync(id);
        if (report == null)
            return NotFound();
        return Ok(report);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCommunityReportDto dto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var report = await _reportService.CreateAsync(dto, Guid.Parse(userId));
        return CreatedAtAction(nameof(GetById), new { id = report.Id }, report);
    }

    [Authorize(Roles = "Admin,LGU,Barangay,Maintenance")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCommunityReportDto dto)
    {
        var report = await _reportService.UpdateAsync(id, dto);
        if (report == null)
            return NotFound();
        return Ok(report);
    }

    [Authorize(Roles = "Admin,LGU,Barangay,Maintenance")]
<<<<<<< HEAD
    [HttpPost("{id}/complete")]
    public async Task<IActionResult> Complete(Guid id, [FromBody] CompleteReportDto dto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var report = await _reportService.CompleteAsync(id, dto, Guid.Parse(userId));
        if (report == null)
            return NotFound();
        return Ok(report);
    }

    [Authorize(Roles = "Admin,LGU,Barangay,Maintenance")]
=======
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _reportService.DeleteAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
