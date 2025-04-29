using ReForm.Core.DTOs;
using ReForm.Core.Models.Metadata;

namespace ReForm.Core.Interfaces;

public interface ITagService
{
    Task<IEnumerable<TagDto>> GetAllTagsAsync();

    Task<Tag> AddTagAsync(string tagName);

    Task<IEnumerable<TagDto>> SearchTagsAsync(string searchTerm);

    Task<TagDto?> GetTagByIdAsync(int id);
}