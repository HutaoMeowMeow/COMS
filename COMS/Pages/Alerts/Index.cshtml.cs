using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using COMS.Services;
using Microsoft.AspNetCore.Authorization;

namespace COMS.Pages.Alerts;

public class IndexModel : PageModel
{
    private readonly IAlertService _alertService;

    public IndexModel(IAlertService alertService)
    {
        _alertService = alertService;
    }

    public async Task<IActionResult> OnGet()
    {
        return Page();
    }
}
