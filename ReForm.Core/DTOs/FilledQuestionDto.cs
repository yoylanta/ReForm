using ReForm.Core.Models.Enums;
using ReForm.Core.Models.Submissions;

namespace ReForm.Core.DTOs;
public class FilledQuestionDto
{
    public int TemplateQuestionId { get; set; }
    public string Text { get; set; }
    public bool IsMandatory { get; set; }
    public QuestionTypeEnum Type { get; set; }
    public string Options { get; set; }
    public List<AnswerDto> Answers { get; set; } = new List<AnswerDto>();

    public FilledQuestionDto(FilledQuestion filledQuestion)
    {
        TemplateQuestionId = filledQuestion.TemplateQuestionId;
        Answers = filledQuestion.Answers
            .Select(answer => new AnswerDto(answer))
            .ToList();
    }

    public FilledQuestionDto() { }
}
