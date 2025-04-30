using ReForm.Core.Models.Templates;

namespace ReForm.Core.DTOs;

public class TemplateFormDto
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public int UserId { get; set; }

    public string? TopicName { get; set; }

    public string? ImageUrl { get; set; }

    public List<string> Tags { get; set; } = [];

    public bool IsPublic { get; set; }

    public List<int> AllowedUserIds { get; set; } = new();

    public TemplateFormDto(TemplateForm templateForm)
    {
        Id = templateForm.Id;
        Title = templateForm.Title;
        Description = templateForm.Description;
        UserId = templateForm.UserId;
        TopicName = templateForm.Topic?.Name ?? string.Empty;
        ImageUrl = templateForm.ImageUrl;
        Tags = templateForm.Tags.Select(t => t.Name).ToList();
        IsPublic = templateForm.IsPublic;
        AllowedUserIds = templateForm
            .AllowedUsers
            .Select(u => u.Id)
            .ToList();
    }

    public TemplateFormDto() {}
}