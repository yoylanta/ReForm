using ReForm.Core.Models.Templates;

namespace ReForm.Core.DTOs;

public class TemplateFormDto
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public int UserId { get; set; }

    public TemplateFormDto(TemplateForm templateForm)
    {
        Id = templateForm.Id;
        Title = templateForm.Title;
        UserId = templateForm.UserId;
    }

    public TemplateFormDto() {}
}