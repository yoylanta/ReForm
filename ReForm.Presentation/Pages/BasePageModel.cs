using Microsoft.AspNetCore.Mvc.RazorPages;
using ReForm.Core.Interfaces;

namespace ReForm.Presentation.Pages;

public class BasePageModel(IUserService userService) : PageModel
{
    protected async Task<bool> RedirectIfNotAuthenticated()
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            var email = User.Identity.Name;
            var user = await userService.GetByEmailAsync(email);

            if (user == null || user.IsBlocked)
            {
                Response.Redirect("/Account/Login");
                return false;
            }
        }
        else
        {
            Response.Redirect("/Account/Login");
            return false;
        }

        return true;
    }
}