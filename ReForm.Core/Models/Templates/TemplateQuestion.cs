using ReForm.Core.Models.Enums;
using ReForm.Core.Models.Submissions;

namespace ReForm.Core.Models.Templates;

public class TemplateQuestion
{
    public int Id { get; set; }
    public string Text { get; set; }
    public QuestionTypeEnum Type { get; set; }

    public string Options { get; set; } = string.Empty;
    public bool IsMandatory { get; set; }
    public int TemplateFormId { get; set; }

    public TemplateForm TemplateForm { get; set; }
    public ICollection<FilledQuestion> ClonedQuestions { get; set; } = [];
}