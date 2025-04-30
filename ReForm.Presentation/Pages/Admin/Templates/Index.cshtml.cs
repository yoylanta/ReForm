using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Identity;
using ReForm.Core.Models.Templates;

namespace ReForm.Presentation.Pages.Admin.Templates;

public class Index(
    IUserService userService,
    UserManager<User> userManager,
    ITemplateService templateService) : BasePageModel(userService)
{
    public List<TemplateFormDto> AdminTemplates { get; set; }

    public async Task OnGetAsync()
    {
        await RedirectIfNotAuthenticated();
        var userId = int.Parse(userManager.GetUserId(User));
        AdminTemplates = (await templateService.GetAllTemplateFormsAsync())
            .Select(t => new TemplateFormDto(t)
            {
                Id = t.Id,
                Title = t.Title,
                UserId = t.UserId
            })
            .ToList() ?? new List<TemplateFormDto>();
    }
}