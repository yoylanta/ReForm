using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Enums;
using ReForm.Core.Models.Metadata;
using ReForm.Core.Models.Templates;
using ReForm.Core.Models.Identity;

namespace ReForm.Infrastructure.Services;

public class TemplateService(
    IEntityRepository<TemplateForm> repository,
    IEntityRepository<TemplateQuestion> questionRepository,
    IEntityRepository<Topic> topicRepository,
    IEntityRepository<Tag> tagRepository,
    IEntityRepository<User> userRepository,
    ITagService tagService,
    UserManager<User> userManager) : ITemplateService
{
    public async Task<IEnumerable<TemplateForm>> GetByUserIdAsync(int userId)
    {
        return await repository.FindAsync(f => f.UserId == userId);
    }

    public async Task<TemplateForm> CreateAsync(TemplateFormDto templateDto)
    {
        var templateForm = new TemplateForm()
        {
            Title = templateDto.Title,
            UserId = templateDto.UserId
        };

        await repository.AddAsync(templateForm);
        await repository.SaveChangesAsync();

        return templateForm;
    }

    public async Task AddQuestionAsync(TemplateQuestionDto questionDto)
    {
        var question = new TemplateQuestion
        {
            Text = questionDto.Text,
            Type = questionDto.Type,
            Options = questionDto.Options,
            IsMandatory = questionDto.IsMandatory,
            TemplateFormId = questionDto.TemplateFormId
        };

        questionRepository.AddAsync(question);
        await questionRepository.SaveChangesAsync();
    }

    public async Task<TemplateForm?> GetTemplateFormWithQuestionsAsync(int id)
    {
        var templateForm = await repository.FirstOrDefaultAsyncWithIncludes(
        p => p.Id == id,
        p => p.Questions,
        p => p.Topic,
        p => p.Tags,
        p => p.AllowedUsers
        );

        return templateForm;
    }

    public async Task DeleteTemplatesAsync(List<int> templateIds)
    {
        var templates = await repository.FindAsync(t => templateIds.Contains(t.Id));
        repository.RemoveRange(templates);
        await repository.SaveChangesAsync();
    }

    public async Task<TemplateQuestionDto?> GetQuestionAsync(int questionId)
    {
        var q = await questionRepository.FirstOrDefaultAsync(x => x.Id == questionId);
        if (q == null) return null;
        return new TemplateQuestionDto(q);
    }

    public async Task<bool> EditQuestionAsync(TemplateQuestionDto questionDto)
    {
        var question = await questionRepository.FirstOrDefaultAsync(q => q.Id == questionDto.Id);
        if (question == null) return false;

        question.Text = questionDto.Text;
        question.Type = questionDto.Type;
        question.Options = questionDto.Options;
        question.IsMandatory = questionDto.IsMandatory;

        questionRepository.Update(question);
        await questionRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteQuestionAsync(int questionId)
    {
        var question = await questionRepository.FirstOrDefaultAsync(q => q.Id == questionId);
        if (question == null) return false;

        questionRepository.Remove(question);
        await questionRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateTemplateFormAsync(TemplateFormDto templateDto)
    {
        var templateForm = await repository.FirstOrDefaultAsyncWithIncludes(
        t => t.Id == templateDto.Id,
        t => t.Tags,
        t => t.AllowedUsers
        );
        if (templateForm == null)
            return false;

        templateForm.Title = templateDto.Title;
        templateForm.Description = templateDto.Description;
        templateForm.ImageUrl = templateDto.ImageUrl;
        templateForm.IsPublic = templateDto.IsPublic;

        Topic? topic = null;

        var newIds = templateDto.AllowedUserIds
            .Distinct()
            .ToList();

        var toRemove = templateForm
            .AllowedUsers
            .Where(u => !newIds.Contains(u.Id))
            .ToList();
        foreach (var u in toRemove)
            templateForm.AllowedUsers.Remove(u);

        foreach (var id in newIds)
        {
            if (templateForm.AllowedUsers.Any(u => u.Id == id))
                continue;

            var user = await userRepository.GetByIdAsync(id);
            if (user != null)
                templateForm.AllowedUsers.Add(user);
        }

        if (!string.IsNullOrWhiteSpace(templateDto.TopicName))
        {
            topic = await topicRepository.FirstOrDefaultAsync(t => t.Name == templateDto.TopicName);
            if (topic == null)
            {
                topic = new Topic { Name = templateDto.TopicName };
                await topicRepository.AddAsync(topic);
                await topicRepository.SaveChangesAsync();
            }
        }

        templateForm.Topic = topic;

        var newTagNames = templateDto.Tags
            .Select(n => n.Trim())
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var tagsToRemove = templateForm.Tags
            .Where(t => !newTagNames.Contains(t.Name, StringComparer.OrdinalIgnoreCase))
            .ToList();
        foreach (var tag in tagsToRemove)
            templateForm.Tags.Remove(tag);

        foreach (var tagName in newTagNames)
        {
            if (templateForm.Tags.Any(t =>
                    t.Name.Equals(tagName, StringComparison.OrdinalIgnoreCase)))
                continue;
            var newTag = await tagService.AddTagAsync(tagName);

            templateForm.Tags.Add(newTag);
        }

        repository.Update(templateForm);
        await repository.SaveChangesAsync();
        return true;
    }

    public async Task<TemplateForm?> GetTemplateFormByIdAsync(int id)
    {
        return await repository.AsQueryable()
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Topic>> GetAllTopicsAsync()
    {
        return await topicRepository.GetAllAsync();
    }

    public async Task<IEnumerable<TemplateForm>> GetAvailableForms(int userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        var isAdmin = await userManager.IsInRoleAsync(user!, UserRolesEnum.Admin.ToString());

        return isAdmin
            ? await repository.AsQueryable().Include(f => f.AllowedUsers).ToListAsync()
            : await repository.AsQueryable()
                .Include(f => f.AllowedUsers)
                .Where(f => f.UserId == userId || f.IsPublic || f.AllowedUsers.Any(au => au.Id == userId))
                .ToListAsync();
    }

    public async Task<IEnumerable<TemplateForm>> GetAllTemplateFormsAsync()
    {
        return await repository.AsQueryable()
            .Include(f => f.AllowedUsers)
            .Include(f => f.Tags)
            .Include(f => f.Topic)
            .ToListAsync();
    }
}