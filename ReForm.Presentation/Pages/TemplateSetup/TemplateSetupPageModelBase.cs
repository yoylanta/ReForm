using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Identity;
using ReForm.Core.Models.Templates;

namespace ReForm.Presentation.Pages.TemplateSetup
{
    public abstract class TemplateSetupPageModelBase(
        ITemplateService templateService,
        UserManager<User> userManager) : PageModel
    {
        protected readonly ITemplateService TemplateService = templateService;
        protected readonly UserManager<User> UserManager = userManager;

        [BindProperty]
        public required TemplateForm Template { get; set; }

        protected async Task<IActionResult?> InitializeAsync(int id)
        {
            Template = await TemplateService.GetTemplateFormWithQuestionsAsync(id);

            if (Template == null)
                return NotFound();

            var current = await UserManager.GetUserAsync(User);
            if (current == null)
                return Forbid();

            var isAdmin = await UserManager.IsInRoleAsync(current, "Admin");
            if (current.Id != Template.UserId && !isAdmin)
                return Forbid();

            return null;
        }
    }
}