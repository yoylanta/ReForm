using ReForm.Core.Models.Metadata;

namespace ReForm.Core.DTOs;

public class TopicDto
{
    public int? Id { get; set; }

    public string Name { get; set; }

    public TopicDto(Topic topic)
    {
        Id = topic.Id;
        Name = topic.Name;
    }

}
