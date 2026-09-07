using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using COMS.Services;
using Microsoft.AspNetCore.Authorization;

namespace COMS.Pages.Canals;

public class IndexModel : PageModel
{
    private readonly ICanalService _canalService;

    public IndexModel(ICanalService canalService)
    {
        _canalService = canalService;
    }

    public async Task<IActionResult> OnGet()
    {
        return Page();
    }
}
