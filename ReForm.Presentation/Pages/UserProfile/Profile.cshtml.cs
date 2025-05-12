using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Identity;
using ReForm.Presentation.Pages.Admin.Users;

namespace ReForm.Presentation.Pages.UserProfile;

public class ProfileModel(ILogger<IndexModel> logger, IUserService userService, UserManager<User> userManager) : BasePageModel(userService)
{
    public UserDto? CurrentUser { get; private set; }
    public async Task<IActionResult> OnGetAsync()
    {
        await RedirectIfNotAuthenticated();
  
        var userId = int.Parse(userManager.GetUserId(User)); 
        CurrentUser = await userService.GetByIdAsync(userId);
        return Page();
    }
}
