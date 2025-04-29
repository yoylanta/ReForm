using Microsoft.AspNetCore.Identity;
using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Identity;

namespace ReForm.Presentation.Pages.TemplateSetup;

public class SubmissionsModel(
    ITemplateService templateService,
    IFilledFormService filledFormService,
    IUserService userService,
    UserManager<User> userManager) : TemplateSetupPageModelBase(templateService, userManager)
{
    public List<FilledFormDto> Submissions { get; set; } = new();
    public Dictionary<int, UserDto> UsersById { get; set; } = new();

    public async Task OnGetAsync(int id)
    {
        await InitializeAsync(id);
        Submissions = (await filledFormService.GetFilledFormsByTemplateIdAsync(id)).ToList();
        var userIds = Submissions.Select(f => f.UserId).Distinct();
        foreach (var userId in userIds)
        {
            var user = await userService.GetByIdAsync(userId);
            if (user != null)
            {
                UsersById[userId] = user;
            }
        }

        ViewData["ActiveTab"] = "Submissions";
    }
}