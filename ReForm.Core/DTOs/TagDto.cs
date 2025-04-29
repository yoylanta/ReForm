using ReForm.Core.Models.Metadata;

namespace ReForm.Core.DTOs;

public class TagDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public TagDto(Tag tag)
    {
        Id = tag.Id;
        Name = tag.Name;
    }

    public TagDto() {}
}