using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Metadata;
using ReForm.Core.Models.Templates;

namespace ReForm.Infrastructure.Services;

public class TopicService(IEntityRepository<TemplateForm> repository, IEntityRepository<TemplateQuestion> questionRepository,
     IEntityRepository<Topic> topicRepository) : ITopicService
{
    public async Task<IEnumerable<TopicDto>> GetAllTopicsAsync()
    {
        var topics = await topicRepository.GetAllAsync();
        return topics.Select(t => new TopicDto(t)).ToList();
    }

    public async Task<TopicDto> AddTopicAsync(string topicName)
    {
        var existingTopic = await topicRepository.FirstOrDefaultAsync(t => t.Name == topicName);
        if (existingTopic != null)
        {
            return new TopicDto(existingTopic);
        }

        var newTopic = new Topic { Name = topicName };
        await topicRepository.AddAsync(newTopic);
        await topicRepository.SaveChangesAsync();

        return new TopicDto(newTopic);
    }

    public async Task<IEnumerable<TopicDto>> SearchTopicsAsync(string searchTerm)
    {
        var topics = await topicRepository.GetAllAsync();
        var filteredTopics = topics.Where(t => t.Name.Contains(searchTerm)).ToList();
        return filteredTopics.Select(t => new TopicDto(t)).ToList();
    }

    public async Task<TopicDto?> GetTopicByIdAsync(int id)
    {
        var topic = await topicRepository.GetByIdAsync(id);
        return topic == null ? null : new TopicDto(topic);
    }
}

