using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Templates;

namespace ReForm.Presentation.Pages.TemplateSetup
{
    public class SubmissionViewModel(ITemplateService templateService, IFilledFormService filledFormService, IUserService userService) : PageModel
    {

        [BindProperty]
        public FilledFormDto FilledForm { get; set; } = null!;

        public TemplateForm Template { get; set; } = null!;

        public string? UserName { get; set; }

        public string? UserEmail { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var filledForm = await filledFormService.GetFilledFormByIdAsync(id);
            if (filledForm == null)
                return NotFound();

            FilledForm = filledForm;

            Template = await templateService.GetTemplateFormWithQuestionsAsync(FilledForm.TemplateFormId)
                ?? throw new InvalidOperationException("Template not found.");

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
