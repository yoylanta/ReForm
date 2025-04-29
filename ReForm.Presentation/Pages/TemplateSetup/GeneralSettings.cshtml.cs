using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReForm.Core.Models.Templates;
using ReForm.Core.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ReForm.Core.Models.Metadata;
using ReForm.Core.DTOs;
using ReForm.Core.Models.Identity;

namespace ReForm.Presentation.Pages.TemplateSetup
{
    public class GeneralSettingsModel(ITemplateService templateService, UserManager<User> userMgr) : TemplateSetupPageModelBase(templateService, userMgr)
    {
        private readonly ITemplateService _templateService = templateService;
        private readonly UserManager<User> _userMgr = userMgr;

        public string InitialTagNames { get; set; } = string.Empty;

        [BindProperty]
        public IEnumerable<Topic> Topics { get; set; } = new List<Topic>();

        [BindProperty]
        public string TagNames { get; set; } = "";

        [BindProperty]
        public string AllowedUsersCsv { get; set; } = "";

        public async Task<IActionResult> OnGetAsync(int id)
        {
            await InitializeAsync(id);
            var current = await _userMgr.GetUserAsync(User);
            Topics = await _templateService.GetAllTopicsAsync();
            InitialTagNames = string.Join(",", Template.Tags.Select(t => t.Name).ToArray());
            AllowedUsersCsv = string.Join(",", Template.AllowedUsers.Select(u => u.Id));
            ViewData["ActiveTab"] = "General Settings";
            return Page();
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
                TopicName = Template.Topic?.Name ?? "",
                Tags = TagNames
                    .Split(',',
                    System.StringSplitOptions
                        .RemoveEmptyEntries |
                    System.StringSplitOptions
                        .TrimEntries)
                    .ToList(),
                AllowedUserIds = AllowedUsersCsv
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToList()
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