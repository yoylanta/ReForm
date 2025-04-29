using Microsoft.AspNetCore.Identity;
using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Enums;
using ReForm.Core.Models.Identity;

namespace ReForm.Infrastructure.Services;

public class UserService(IEntityRepository<User> userRepository, UserManager<User> userManager) : IUserService
{
    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        var user = await userRepository.FirstOrDefaultAsync(u => u.Email == email);
        return user != null ? new UserDto(user) : null;
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = await userRepository.GetAllAsync();
        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var isAdmin = await userManager.IsInRoleAsync(user, UserRolesEnum.Admin.ToString());
            userDtos.Add(new UserDto(user, isAdmin));
        }

        return userDtos.OrderByDescending(u => u.LastLogin).ToList();
    }

    public async Task BlockUsersAsync(IEnumerable<int> userIds)
    {
        var users = await userRepository.FindAsync(u => userIds.Contains(u.Id));
        foreach (var user in users)
        {
            user.IsBlocked = true;
            userRepository.Update(user);
        }
        await userRepository.SaveChangesAsync();
    }

    public async Task UnblockUsersAsync(IEnumerable<int> userIds)
    {
        var users = await userRepository.FindAsync(u => userIds.Contains(u.Id));
        foreach (var user in users)
        {
            user.IsBlocked = false;
            userRepository.Update(user);
        }
        await userRepository.SaveChangesAsync();
    }

    public async Task DeleteUsersAsync(IEnumerable<int> userIds)
    {
        var users = await userRepository.FindAsync(u => userIds.Contains(u.Id));
        userRepository.RemoveRange(users);
        await userRepository.SaveChangesAsync();
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await userRepository.GetByIdAsync(id);
        return user != null ? new UserDto(user) : null;
    }

    public async Task<List<UserDto>> SearchUsersAsync(string searchTerm)
    {
        return (await userRepository
                .FindAsync(u => u.UserName.Contains(searchTerm)
                                || u.Email.Contains(searchTerm)))
            .Select(u => new UserDto(u))
            .ToList();
    }

    public async Task ChangeUsersRole(IEnumerable<int> userIds)
    {
        var users = await userRepository.FindAsync(u => userIds.Contains(u.Id));
        foreach (var user in users)
        {
            var currentRole = (await userManager.GetRolesAsync(user)).First();
            await userManager.RemoveFromRoleAsync(user, currentRole);
            var newRole = currentRole == UserRolesEnum.Admin.ToString()
                ? UserRolesEnum.User.ToString()
                : UserRolesEnum.Admin.ToString();

            await userManager.AddToRoleAsync(user, newRole);
        }
    }

}