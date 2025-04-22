using ReForm.Core.Models.Enums;
using ReForm.Core.Models.Templates;

namespace ReForm.Core.Models.Submissions;

public class FilledQuestion
{
    public int Id { get; set; }
    public string Text { get; set; } = null!;
    public QuestionTypeEnum Type { get; set; }
    public string Options { get; set; } = string.Empty;
    public bool IsMandatory { get; set; }

    public int FilledFormId { get; set; }
    public int TemplateQuestionId { get; set; }

    public FilledForm FilledForm { get; set; } = null!;
    public TemplateQuestion TemplateQuestion { get; set; } = null!;
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
}