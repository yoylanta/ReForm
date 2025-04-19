using ReForm.Core.DTOs;
using ReForm.Core.Models;
using ReForm.Core.Models.Identity;

namespace ReForm.Core.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> GetAllUsersAsync();

    Task BlockUsersAsync(IEnumerable<int> userIds);

    Task UnblockUsersAsync(IEnumerable<int> userIds);

    Task DeleteUsersAsync(IEnumerable<int> userIds);

    Task<UserDto?> GetByEmailAsync(string email);
}