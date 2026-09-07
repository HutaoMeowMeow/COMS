using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace COMS.Pages.Auth;

public class RegisterModel : PageModel
{
    public IActionResult OnGet()
    {
        return Page();
    }
}
