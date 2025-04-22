using ReForm.Core.Models.Submissions;

namespace ReForm.Core.DTOs;

public class AnswerDto
{
    public int Id { get; set; }
    public string Response { get; set; } = string.Empty;
    public int UserId { get; set; }

    public AnswerDto(Answer answer)
    {
        Id = answer.Id;
        Response = answer.Response;
        UserId = answer.UserId;
    }

    public AnswerDto() { }
}

