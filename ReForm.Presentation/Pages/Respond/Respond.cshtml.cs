using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Templates;

namespace ReForm.Presentation.Pages.Respond;

[Authorize]
public class RespondModel : PageModel
{
    private readonly ITemplateService _templateService;

    public RespondModel(ITemplateService templateService)
    {
        _templateService = templateService;
    }

    [BindProperty]
    public TemplateForm Template { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Template = await _templateService.GetTemplateFormWithQuestionsAsync(id);
        if (Template == null) return NotFound();
        return Page();
    }
}
