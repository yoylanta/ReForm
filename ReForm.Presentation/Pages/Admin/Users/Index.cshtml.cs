using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Enums;
using ReForm.Core.Models.Identity;

namespace ReForm.Presentation.Pages.Admin.Users;

public class IndexModel(ILogger<IndexModel> logger, IUserService userService, UserManager<User> userManager) : BasePageModel(userService)
{
    public List<UserDto> Users { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        await RedirectIfNotAuthenticated();
        var current = await userManager.GetUserAsync(this.User);
        var isAdmin = await userManager.IsInRoleAsync(current!, UserRolesEnum.Admin.ToString());
        if (!isAdmin) return Forbid();
        Users = await userService.GetAllUsersAsync();
        return Page();
    }
}