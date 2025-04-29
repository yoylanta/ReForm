using ReForm.Core.DTOs;
using ReForm.Core.Models.Metadata;

namespace ReForm.Core.Interfaces;

public interface ITopicService
{
    Task<IEnumerable<TopicDto>> GetAllTopicsAsync();

    Task<TopicDto> AddTopicAsync(string topicName);

    Task<IEnumerable<TopicDto>> SearchTopicsAsync(string searchTerm);

    Task<TopicDto?> GetTopicByIdAsync(int id);
}
