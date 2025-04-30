using ReForm.Core.Models.Identity;
using ReForm.Core.Models.Templates;

namespace ReForm.Core.Models.Submissions;

public class FilledForm
{
    public int Id { get; set; }

    public string Title { get; set; }

    public int TemplateFormId { get; set; }

    public int UserId { get; set; }

    public DateTimeOffset SubmittedAt { get; set; }

    public User User { get; set; }

    public TemplateForm TemplateForm { get; set; }

    public ICollection<FilledQuestion> Questions { get; set; } = new List<FilledQuestion>();
}