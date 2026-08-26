using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using COMS.Services;
using Microsoft.AspNetCore.Authorization;

namespace COMS.Pages.Risk;

public class IndexModel : PageModel
{
    private readonly IFloodRiskService _riskService;

    public IndexModel(IFloodRiskService riskService)
    {
        _riskService = riskService;
    }

    public async Task<IActionResult> OnGet()
    {
        return Page();
    }
}
