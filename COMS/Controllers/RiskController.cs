using COMS.DTOs;
using COMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COMS.Controllers;

[ApiController]
[Route("api/[controller]")]
<<<<<<< HEAD
[Authorize(Roles = "Admin,LGU,Barangay")]
=======
[Authorize(Roles = "Admin,LGU,Barangay,Maintenance,Resident")]
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
public class RiskController : ControllerBase
{
    private readonly IFloodRiskService _riskService;

    public RiskController(IFloodRiskService riskService)
    {
        _riskService = riskService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var assessments = await _riskService.GetAllAsync();
        return Ok(assessments);
    }

    [HttpGet("canal/{canalId}")]
    public async Task<IActionResult> GetByCanal(Guid canalId)
    {
        var assessments = await _riskService.GetByCanalAsync(canalId);
        return Ok(assessments);
    }

    [HttpPost("canal/{canalId}/assess")]
    public async Task<IActionResult> CreateAssessment(Guid canalId, [FromBody] CreateAssessmentDto dto)
    {
        var assessment = await _riskService.CreateAssessmentAsync(
            canalId, dto.RiskLevel, dto.RiskScore, dto.PredictionDetails, dto.ModelVersion);
        return CreatedAtAction(nameof(GetByCanal), new { canalId }, assessment);
    }

    [HttpGet("analytics")]
    public async Task<IActionResult> GetAnalytics()
    {
        var analytics = await _riskService.GetAnalyticsAsync();
        return Ok(analytics);
    }

    [HttpGet("analytics/canal/{canalId}")]
    public async Task<IActionResult> GetAnalyticsByCanal(Guid canalId)
    {
        var analytics = await _riskService.GetAnalyticsByCanalAsync(canalId);
        if (analytics == null)
            return NotFound();
        return Ok(analytics);
    }
}
