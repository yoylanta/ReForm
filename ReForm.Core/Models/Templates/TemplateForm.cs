using ReForm.Core.Models.Identity;
using ReForm.Core.Models.Submissions;
using ReForm.Core.Models.Metadata;

namespace ReForm.Core.Models.Templates;

public class TemplateForm
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public int UserId { get; set; }

    public User User { get; set; }

    public Topic? Topic { get; set; }

    public string? ImageUrl { get; set; }

    public ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public ICollection<TemplateQuestion> Questions { get; set; } = [];

    public ICollection<FilledForm> FilledForms { get; set; } = [];

    public bool IsPublic { get; set; }

    public ICollection<User> AllowedUsers { get; set; } = new List<User>();

}