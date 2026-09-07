using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using COMS.Services;
using Microsoft.AspNetCore.Authorization;

namespace COMS.Pages.Reports;

public class IndexModel : PageModel
{
    private readonly ICommunityReportService _reportService;

    public IndexModel(ICommunityReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<IActionResult> OnGet()
    {
        return Page();
    }
}
