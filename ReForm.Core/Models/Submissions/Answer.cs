using ReForm.Core.Models.Identity;

namespace ReForm.Core.Models.Submissions;

public class Answer
{
    public int Id { get; set; }
    public string Response { get; set; } = null!;
    public int FilledQuestionId { get; set; }
    public int UserId { get; set; }

    public FilledQuestion FilledQuestion { get; set; } = null!;
    public User User { get; set; } = null!;
}