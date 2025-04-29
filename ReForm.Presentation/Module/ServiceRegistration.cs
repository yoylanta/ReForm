using Microsoft.AspNetCore.Identity;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Identity;
using ReForm.Core.Models.Templates;
using ReForm.Core.Models.Submissions;
using ReForm.Infrastructure.Repositories;
using ReForm.Infrastructure.Services;
using ReForm.Core.Models.Metadata;

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
        services.AddScoped<IEntityRepository<FilledForm>, EntityRepository<FilledForm>>();
        services.AddScoped<IEntityRepository<FilledQuestion>, EntityRepository<FilledQuestion>>();
        services.AddScoped<IEntityRepository<Answer>, EntityRepository<Answer>>();
        services.AddScoped<IFilledFormService, FilledFormService>();
        services.AddScoped<IEntityRepository<Tag>, EntityRepository<Tag>>();
        services.AddScoped<IEntityRepository<Topic>, EntityRepository<Topic>>();
        services.AddScoped<ITopicService, TopicService>();
        services.AddScoped<ITagService, TagService>();
    }
}