using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Identity;

namespace ReForm.Presentation.Pages.TemplateSetup;

public class EditModel(ITemplateService templateService, UserManager<User> userManager) : TemplateSetupPageModelBase(templateService, userManager)
{
    public async Task<IActionResult> OnGetAsync(int id)
    {
        await InitializeAsync(id);
        ViewData["ActiveTab"] = "Edit";

        return Page();
    }
}