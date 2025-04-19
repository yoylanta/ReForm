using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;

namespace ReForm.Presentation.Pages.Admin.Users;

public class IndexModel(ILogger<IndexModel> logger, IUserService userService) : BasePageModel(userService)
{
    public List<UserDto> Users { get; set; }

    public async Task OnGetAsync()
    {
        await RedirectIfNotAuthenticated();
        Users = await userService.GetAllUsersAsync();
    }
}