using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using COMS.Services;
using COMS.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace COMS.Pages.Dashboard;

public class IndexModel : PageModel
{
    private readonly ICanalService _canalService;
    private readonly IAlertService _alertService;
    private readonly ICommunityReportService _reportService;
    private readonly IUserService _userService;
    private readonly INotificationService _notificationService;

    public IndexModel(
        ICanalService canalService,
        IAlertService alertService,
        ICommunityReportService reportService,
        IUserService userService,
        INotificationService notificationService)
    {
        _canalService = canalService;
        _alertService = alertService;
        _reportService = reportService;
        _userService = userService;
        _notificationService = notificationService;
    }

    public DashboardSummaryDto Summary { get; set; } = new();

    public async Task OnGet()
    {
        var canals = await _canalService.GetAllAsync();
        var activeAlerts = await _alertService.GetActiveAsync();
        var reports = await _reportService.GetAllAsync();
        var users = await _userService.GetAllAsync();
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var unreadNotifications = userId != null
            ? await _notificationService.GetUnreadCountAsync(Guid.Parse(userId))
            : 0;

        Summary = new DashboardSummaryDto
        {
            TotalCanals = canals.Count(),
            TotalSensors = canals.Sum(c => c.SensorCount),
            ActiveAlerts = activeAlerts.Count(),
            PendingReports = reports.Count(r => r.Status == "Pending"),
            TotalUsers = users.Count(u => u.IsActive),
            UnreadNotifications = unreadNotifications
        };
    }
}
