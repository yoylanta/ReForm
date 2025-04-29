using ReForm.Core.DTOs;
using ReForm.Core.Models.Metadata;
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

    Task<bool> EditQuestionAsync(TemplateQuestionDto questionDto);

    Task<bool> DeleteQuestionAsync(int questionId);

    Task<bool> UpdateTemplateFormAsync(TemplateFormDto templateDto);

    Task<TemplateForm?> GetTemplateFormByIdAsync(int id);

    Task<IEnumerable<Topic>> GetAllTopicsAsync();

    Task<IEnumerable<TemplateForm>> GetAvailableForms(int userId);
}