using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Templates;

namespace ReForm.Infrastructure.Services;

public class TemplateService(IEntityRepository<TemplateForm> repository, IEntityRepository<TemplateQuestion> questionRepository) : ITemplateService
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
}