using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Identity;

namespace ReForm.Presentation.Pages;

[Authorize]
public class Index(
    IUserService userService,
    UserManager<User> userManager,
    ITemplateService templateService) : BasePageModel(userService)
{
    public List<TemplateFormDto> MyTemplates { get; set; }

    public async Task OnGetAsync()
    {
        await RedirectIfNotAuthenticated();
        var userId = int.Parse(userManager.GetUserId(User));
        MyTemplates = (await templateService.GetByUserIdAsync(userId))
            .Select(t => new TemplateFormDto(t)
            {
                Id = t.Id,
                Title = t.Title,
                UserId = t.UserId
            })
            .ToList() ?? new List<TemplateFormDto>();
    }
}