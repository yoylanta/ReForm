using ReForm.Core.Models.Submissions;

namespace ReForm.Core.DTOs;
public class FilledQuestionDto
{
    public int TemplateQuestionId { get; set; }
    public List<AnswerDto> Answers { get; set; } = new List<AnswerDto>();  // Change to List<AnswerDto>

    public FilledQuestionDto(FilledQuestion filledQuestion)
    {
        TemplateQuestionId = filledQuestion.TemplateQuestionId;
        Answers = filledQuestion.Answers
            .Select(answer => new AnswerDto(answer))  // Create AnswerDto for each Answer
            .ToList();
    }

    public FilledQuestionDto() { }
}
