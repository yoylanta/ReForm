using Microsoft.AspNetCore.Identity;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Identity;
using ReForm.Core.Models.Templates;
using ReForm.Infrastructure.Repositories;
using ReForm.Infrastructure.Services;

namespace ReForm.Presentation.Module;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IEntityRepository<User>, EntityRepository<User>>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEntityRepository<TemplateForm>, EntityRepository<TemplateForm>>();
        services.AddScoped<IEntityRepository<TemplateQuestion>, EntityRepository<TemplateQuestion>>();
        services.AddScoped<ITemplateService, TemplateService>();
        services.AddTransient<IUserValidator<User>, CustomUserValidator>();
    }
}