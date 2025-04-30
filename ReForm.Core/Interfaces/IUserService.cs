using ReForm.Core.DTOs;

namespace ReForm.Core.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetByIdAsync(int id);
    Task<List<UserDto>> GetAllUsersAsync();

    Task BlockUsersAsync(IEnumerable<int> userIds);

    Task UnblockUsersAsync(IEnumerable<int> userIds);

    Task DeleteUsersAsync(IEnumerable<int> userIds);

    Task<UserDto?> GetByEmailAsync(string email);

    Task<List<UserDto>> SearchUsersAsync(string searchTerm);

    Task ChangeUsersRole(IEnumerable<int> userIds);
}