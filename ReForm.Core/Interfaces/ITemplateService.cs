using ReForm.Core.DTOs;
using ReForm.Core.Models.Templates;

namespace ReForm.Core.Interfaces;

public interface ITemplateService
{
    Task<IEnumerable<TemplateForm>> GetByUserIdAsync(int userId);

    Task<TemplateForm> CreateAsync(TemplateFormDto templateDto);

    Task<TemplateForm?> GetTemplateFormWithQuestionsAsync(int id);

    Task AddQuestionAsync(TemplateQuestionDto question);

    Task DeleteTemplatesAsync(List<int> templateIds, int userId);

    Task<TemplateQuestionDto?> GetQuestionAsync(int questionId);

    Task<bool> EditQuestionAsync(int questionId, string newText);

    Task<bool> DeleteQuestionAsync(int questionId);
}