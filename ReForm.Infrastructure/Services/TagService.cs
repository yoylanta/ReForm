using Microsoft.EntityFrameworkCore;
using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Metadata;

namespace ReForm.Infrastructure.Services;

public class TagService(IEntityRepository<Tag> tagRepository) : ITagService
{
    public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
    {
        var tags = await tagRepository.GetAllAsync();
        return tags.Select(t => new TagDto(t)).ToList();
    }

    public async Task<Tag> AddTagAsync(string tagName)
    {
        if (string.IsNullOrWhiteSpace(tagName))
            throw new ArgumentException("Tag name cannot be empty.", nameof(tagName));
        var trimmed = tagName.Trim().ToLower();

        var existingTag = await tagRepository
            .AsQueryable()
            .FirstOrDefaultAsync(t => t.Name.ToLower() == trimmed);


        if (existingTag != null)
        {
            return existingTag;
        }

        var newTag = new Tag { Name = tagName.Trim() };
        await tagRepository.AddAsync(newTag);
        await tagRepository.SaveChangesAsync();

        return newTag;
    }


    public async Task<IEnumerable<TagDto>> SearchTagsAsync(string searchTerm)
    {
        var tags = await tagRepository.GetAllAsync();
        var filteredTags = tags.Where(t => t.Name.Contains(searchTerm)).ToList();
        return filteredTags.Select(t => new TagDto(t)).ToList();
    }

    public async Task<TagDto?> GetTagByIdAsync(int id)
    {
        var tag = await tagRepository.GetByIdAsync(id);
        return tag == null ? null : new TagDto(tag);
    }
}