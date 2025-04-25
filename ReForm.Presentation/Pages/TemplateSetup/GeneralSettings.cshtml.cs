using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReForm.Core.Models.Templates;
using ReForm.Core.Interfaces;
using System.Threading.Tasks;
using ReForm.Core.Models.Metadata;
using ReForm.Core.DTOs;

namespace ReForm.Presentation.Pages.TemplateSetup
{
    public class GeneralSettingsModel : PageModel
    {
        private readonly ITemplateService _templateService;

        public GeneralSettingsModel(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        [BindProperty]
        public TemplateForm Template { get; set; } = null!;
        public IEnumerable<Topic> Topics { get; set; } = new List<Topic>();

        public async Task OnGetAsync(int id)
        {
            Template = await _templateService.GetTemplateFormByIdAsync(id);
            Topics = await _templateService.GetAllTopicsAsync();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Template == null)
            {
                return NotFound();
            }

            Template.Title = Template.Title;
            Template.Description = Template.Description;
            Template.IsPublic = Template.IsPublic;

            var templateDto = new TemplateFormDto
            {
                Id = Template.Id,
                Title = Template.Title,
                Description = Template.Description,
                IsPublic = Template.IsPublic,
                UserId = Template.UserId,
                ImageUrl = Template.ImageUrl,
                TopicName = Template.Topic?.Name ?? ""
            };

            var result = await _templateService.UpdateTemplateFormAsync(templateDto);

            if (result)
            {
                ViewData["ActiveTab"] = "General Settings";
                return RedirectToPage("/TemplateSetup/GeneralSettings", new { id = Template.Id });
            }

            return BadRequest("Failed to update the template.");
        }
    }
}
