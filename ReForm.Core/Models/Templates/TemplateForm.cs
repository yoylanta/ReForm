using ReForm.Core.Models.Identity;
using ReForm.Core.Models.Submissions;

namespace ReForm.Core.Models.Templates;

public class TemplateForm
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int UserId { get; set; }

    public User User { get; set; } = null!;
    public ICollection<TemplateQuestion> Questions { get; set; } = [];
    public ICollection<FilledForm> FilledForms { get; set; } = [];
}