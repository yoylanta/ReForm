using ReForm.Core.Models.Submissions;

namespace ReForm.Core.DTOs;

public class FilledFormDto
{
    public int TemplateFormId { get; set; }
    public List<FilledQuestionDto> Questions { get; set; } = new List<FilledQuestionDto>();

    public FilledFormDto() { }

    public FilledFormDto(FilledForm filledForm)
    {
        TemplateFormId = filledForm.TemplateFormId;
        Questions = filledForm.Questions
            .Select(filledQuestion => new FilledQuestionDto(filledQuestion))  // Map to FilledQuestionDto
            .ToList();
    }
}
