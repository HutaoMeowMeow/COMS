using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace COMS.Pages.Auth;

public class RegisterModel : PageModel
{
    public IActionResult OnGet()
    {
<<<<<<< HEAD
        if (Request.Cookies.ContainsKey("coms_token"))
            return RedirectToPage("/Dashboard");
=======
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
        return Page();
    }
}
