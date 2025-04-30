using Microsoft.AspNetCore.Identity;
using ReForm.Core.Models.Identity;

namespace ReForm.Infrastructure.Services;

public class CustomUserValidator : IUserValidator<User>
{
    public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
    {
        return Task.FromResult(IdentityResult.Success);
    }
}