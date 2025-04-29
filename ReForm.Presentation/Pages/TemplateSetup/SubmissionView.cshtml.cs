using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Identity;
using ReForm.Core.Models.Templates;

namespace ReForm.Presentation.Pages.TemplateSetup
{
    public class SubmissionViewModel(ITemplateService templateService, IFilledFormService filledFormService, IUserService userService, UserManager<User> userManager) : TemplateSetupPageModelBase(templateService, userManager)
    {

        [BindProperty]
        public required FilledFormDto FilledForm { get; set; }

        public string? UserName { get; set; }

        public string? UserEmail { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var filledForm = await filledFormService.GetFilledFormByIdAsync(id);
            if (filledForm == null)
                return NotFound();

            FilledForm = filledForm;
            await InitializeAsync(filledForm.TemplateFormId);

            var user = await userService.GetByIdAsync(filledForm.UserId);
            if (user != null)
            {
                UserName = user.Name;
                UserEmail = user.Email;
            }

            ViewData["ActiveTab"] = "Submissions";

            return Page();
        }

    }
}