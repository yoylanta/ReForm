using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Templates;

namespace ReForm.Presentation.Pages.Respond;

[Authorize]
public class RespondModel(ITemplateService templateService) : PageModel
{

    [BindProperty]
    public TemplateForm Template { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Template = await templateService.GetTemplateFormWithQuestionsAsync(id);
        if (Template == null) return NotFound();
        return Page();
    }
}
