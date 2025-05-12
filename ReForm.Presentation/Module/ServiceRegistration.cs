using Microsoft.AspNetCore.Identity;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Identity;
using ReForm.Core.Models.Templates;
using ReForm.Core.Models.Submissions;
using ReForm.Infrastructure.Repositories;
using ReForm.Infrastructure.Services;
using ReForm.Core.Models.Metadata;
using ReForm.Core.Options;

namespace ReForm.Presentation.Module;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services,  IConfiguration configuration)
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
        services.AddHttpClient<ISalesforceService, SalesforceService>();
        services.Configure<SalesforceOptions>(configuration.GetSection("Salesforce"));
    }
}