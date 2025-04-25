using Microsoft.EntityFrameworkCore;
using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Metadata;
using ReForm.Core.Models.Templates;

namespace ReForm.Infrastructure.Services;

public class TemplateService(IEntityRepository<TemplateForm> repository, IEntityRepository<TemplateQuestion> questionRepository,
     IEntityRepository<Topic> topicRepository) : ITemplateService
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
            UserId = templateDto.UserId,
            Description = templateDto.Description,
            IsPublic = templateDto.IsPublic
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
        p => p.Questions
        );

        return templateForm;
    }

    public async Task DeleteTemplatesAsync(List<int> templateIds, int userId)
    {
        var templates = await repository.FindAsync(t => templateIds.Contains(t.Id) && t.UserId == userId);

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
        var templateForm = await repository.FirstOrDefaultAsync(t => t.Id == templateDto.Id);

        if (templateForm == null) return false;

        templateForm.Title = templateDto.Title;
        templateForm.Description = templateDto.Description;

        if (string.IsNullOrWhiteSpace(templateDto.TopicName))
        {
            return false;
        }

        var existingTopic = await topicRepository.FirstOrDefaultAsync(t => t.Name == templateDto.TopicName);
        if (existingTopic == null)
        {
            var newTopic = new Topic { Name = templateDto.TopicName };
            await topicRepository.AddAsync(newTopic);
            await topicRepository.SaveChangesAsync();

            templateForm.Topic = newTopic;
        }
        else
        {
            templateForm.Topic = existingTopic;
        }
        templateForm.ImageUrl = templateDto.ImageUrl;
        templateForm.IsPublic = templateDto.IsPublic;

        repository.Update(templateForm);
        await repository.SaveChangesAsync();
        return true;
    }

    public async Task<TemplateForm?> GetTemplateFormByIdAsync(int id)
    {
        return await repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Topic>> GetAllTopicsAsync()
    {
        return await topicRepository.GetAllAsync();
    }
}