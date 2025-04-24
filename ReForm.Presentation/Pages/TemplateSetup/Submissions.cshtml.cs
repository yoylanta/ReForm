using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Templates;

namespace ReForm.Presentation.Pages.TemplateSetup;

public class SubmissionsModel(ITemplateService templateService, IFilledFormService filledFormService,
    IUserService userService) : PageModel
{
    public TemplateForm Template { get; set; } = null!;
    public List<FilledFormDto> Submissions { get; set; } = new();
    public Dictionary<int, UserDto> UsersById { get; set; } = new();

    public async Task OnGetAsync(int id)
    {
        Template = await templateService.GetTemplateFormWithQuestionsAsync(id)
                   ?? throw new InvalidOperationException("Template not found.");

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
