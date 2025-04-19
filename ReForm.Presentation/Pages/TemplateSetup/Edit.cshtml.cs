using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Templates;

namespace ReForm.Presentation.Pages.TemplateSetup;

public class EditModel(ITemplateService templateService) : PageModel
{
    private readonly ITemplateService _templateService = templateService;

    [BindProperty]
    public TemplateForm Template { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Template = await _templateService.GetTemplateFormWithQuestionsAsync(id);
        if (Template == null)
        {
            return NotFound();
        }

        return Page();
    }
}