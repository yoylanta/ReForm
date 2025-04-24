using ReForm.Core.Models.Identity;
using ReForm.Core.Models.Templates;

namespace ReForm.Core.Models.Submissions;

public class FilledForm
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int TemplateFormId { get; set; }

    public int UserId { get; set; }

    public DateTimeOffset SubmittedAt { get; set; }

    public User User { get; set; } = null!;

    public TemplateForm TemplateForm { get; set; } = null!;

    public ICollection<FilledQuestion> Questions { get; set; } = new List<FilledQuestion>();
}