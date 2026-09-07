using COMS.DTOs;
using COMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COMS.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,LGU,Barangay,Maintenance,Resident")]
public class DashboardController : ControllerBase
{
    private readonly ICanalService _canalService;
    private readonly IFloodRiskService _riskService;
    private readonly IAlertService _alertService;
    private readonly ICommunityReportService _reportService;
    private readonly IUserService _userService;
    private readonly INotificationService _notificationService;

    public DashboardController(
        ICanalService canalService,
        IFloodRiskService riskService,
        IAlertService alertService,
        ICommunityReportService reportService,
        IUserService userService,
        INotificationService notificationService)
    {
        _canalService = canalService;
        _riskService = riskService;
        _alertService = alertService;
        _reportService = reportService;
        _userService = userService;
        _notificationService = notificationService;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var canals = await _canalService.GetAllAsync();
        var activeAlerts = await _alertService.GetActiveAsync();
        var reports = await _reportService.GetAllAsync();
        var users = await _userService.GetAllAsync();

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var unreadNotifications = userId != null
            ? await _notificationService.GetUnreadCountAsync(Guid.Parse(userId))
            : 0;

        var summary = new DashboardSummaryDto
        {
            TotalCanals = canals.Count(),
            TotalSensors = canals.Sum(c => c.SensorCount),
            ActiveAlerts = activeAlerts.Count(),
            PendingReports = reports.Count(r => r.Status == "Pending"),
            TotalUsers = users.Count(u => u.IsActive),
            UnreadNotifications = unreadNotifications
        };

        return Ok(summary);
    }

    [HttpGet("canals-status")]
    public async Task<IActionResult> GetCanalsStatus()
    {
        var statuses = await _canalService.GetStatusAsync();
        return Ok(statuses);
    }

    [HttpGet("analytics")]
    public async Task<IActionResult> GetAnalytics()
    {
        var analytics = await _riskService.GetAnalyticsAsync();
        return Ok(analytics);
    }
}
