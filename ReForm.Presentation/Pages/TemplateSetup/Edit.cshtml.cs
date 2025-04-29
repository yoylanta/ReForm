using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Identity;
using ReForm.Core.Models.Templates;

namespace ReForm.Presentation.Pages.TemplateSetup;

public class EditModel(ITemplateService templateService, UserManager<User> userManager) : TemplateSetupPageModelBase(templateService, userManager)
{
    private readonly ITemplateService _templateService = templateService;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        await InitializeAsync(id);
        ViewData["ActiveTab"] = "Edit";

        return Page();
    }
}