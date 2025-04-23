using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Identity;

namespace ReForm.Presentation.Pages.Respond;

    [Authorize]
    public class RespondIndexModel(
    IUserService userService,
    UserManager<User> userManager,
    ITemplateService templateService) : BasePageModel(userService)
    {
        public List<TemplateFormDto> AvailableTemplates { get; set; }

        public async Task OnGetAsync()
        {

            await RedirectIfNotAuthenticated();
            var userId = int.Parse(userManager.GetUserId(User));
            AvailableTemplates = (await templateService.GetByUserIdAsync(userId))
                .Select(t => new TemplateFormDto(t)
                {
                    Id = t.Id,
                    Title = t.Title,
                    UserId = t.UserId
                })
                .ToList() ?? new List<TemplateFormDto>();
        }
    }
