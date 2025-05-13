using Microsoft.AspNetCore.Mvc;
using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Identity;
using Microsoft.AspNetCore.Identity;
using ReForm.Presentation.Pages.Admin.Users;
using Microsoft.Extensions.Logging;
using ReForm.Infrastructure.Services;

namespace ReForm.Presentation.Pages.UserProfile
{
    public class ProfileModel(ILogger<IndexModel> logger, IUserService userService, UserManager<User> userManager,
        ISalesforceService salesforceService) : BasePageModel(userService)
    {
        public UserDto? CurrentUser { get; private set; }

        [BindProperty]
        public SalesforceDto SalesforceDto { get; set; }

        [BindProperty]
        public string? AuthorizationCode { get; set; }

        public async Task<IActionResult> OnGetAsync(string? code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                // Perform other actions like fetching user info, etc.
                var userId = int.Parse(userManager.GetUserId(User));
                CurrentUser = await userService.GetByIdAsync(userId);
                AuthorizationCode = code;

                return Page();
            }

            // No code, redirect to Salesforce authorization page
            var authorizationUrl = salesforceService.GetSalesforceAuthorizationUrl();
            return Redirect(authorizationUrl);
        }


        public async Task<IActionResult> OnPostAsync()
        {
            // Ensure you're still authenticated
            await RedirectIfNotAuthenticated();

            // If no authorization code, redirect back to login or display error
            if (string.IsNullOrEmpty(AuthorizationCode))
            {
                ModelState.AddModelError(string.Empty, "Authorization code is missing.");
                return Page();
            }

            try
            {
                // Pass the authorization code along with the SalesforceDto to the service method
                await salesforceService.CreateAccountAndContactAsync(SalesforceDto, AuthorizationCode);
                TempData["Success"] = "Salesforce Account & Contact created!";
            }
            catch (Exception ex)
            {
                // Surface any errors back to the form
                ModelState.AddModelError(string.Empty, $"Salesforce error: {ex.Message}");
                return Page();
            }

            // Redirect back to GET so we clear the form and show the success message
            return RedirectToPage("/UserProfile/Profile");
        }
    }
}
