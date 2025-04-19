using ReForm.Core.Models.Enums;
using ReForm.Core.Models.Templates;

namespace ReForm.Core.DTOs;

public class TemplateQuestionDto
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public QuestionTypeEnum Type { get; set; }

    public string Options { get; set; } = string.Empty;

    public bool IsMandatory { get; set; }

    public int TemplateFormId { get; set; }

    public TemplateQuestionDto(TemplateQuestion templateQuestion)
    {
        Id = templateQuestion.Id;
        Text = templateQuestion.Text;
        Type = templateQuestion.Type;
        Options = templateQuestion.Options;
        IsMandatory = templateQuestion.IsMandatory;
        TemplateFormId = templateQuestion.TemplateFormId;
    }

    public TemplateQuestionDto() {}
}