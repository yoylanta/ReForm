using ReForm.Core.Models.Submissions;
using ReForm.Core.Models.Templates;

namespace ReForm.Core.DTOs;

public class FilledFormDto
{
    public int TemplateFormId { get; set; }
    public string Title { get; set; } = "";

    public int UserId { get; set; }
    public List<FilledQuestionDto> Questions { get; set; } = new List<FilledQuestionDto>();

    public FilledFormDto() { }

    public FilledFormDto(FilledForm filledForm)
    {
        TemplateFormId = filledForm.TemplateFormId;
        Title = filledForm.Title;
        Questions = filledForm.Questions
            .Select(filledQuestion => new FilledQuestionDto(filledQuestion))
        .ToList();

        UserId = filledForm.UserId;
    }
}
