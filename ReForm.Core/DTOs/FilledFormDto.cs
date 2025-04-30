using ReForm.Core.Models.Submissions;

namespace ReForm.Core.DTOs;

public class FilledFormDto
{
    public int Id { get; set; }

    public int TemplateFormId { get; set; }

    public string Title { get; set; } = "";

    public int UserId { get; set; }

    public DateTimeOffset SubmittedAt { get; set; }

    public List<FilledQuestionDto> Questions { get; set; } = new List<FilledQuestionDto>();

    public FilledFormDto() {}

    public FilledFormDto(FilledForm filledForm)
    {
        Id = filledForm.Id;
        TemplateFormId = filledForm.TemplateFormId;
        Title = filledForm.Title;
        SubmittedAt = filledForm.SubmittedAt;
        Questions = filledForm.Questions
            .Select(filledQuestion => new FilledQuestionDto(filledQuestion))
            .ToList();
        UserId = filledForm.UserId;
    }
}