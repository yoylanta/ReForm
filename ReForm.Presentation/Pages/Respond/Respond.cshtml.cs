using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Markdig;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Templates;

namespace ReForm.Presentation.Pages.Respond;

[Authorize]
public class RespondModel(ITemplateService templateService) : PageModel
{

    [BindProperty]
    public required TemplateForm Template { get; set; }

    [BindProperty]
    public string FormattedDescription { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Template = await templateService.GetTemplateFormWithQuestionsAsync(id);
        if (Template == null) return NotFound();
        FormattedDescription = Markdown.ToHtml(Template.Description);
        return Page();
    }
}