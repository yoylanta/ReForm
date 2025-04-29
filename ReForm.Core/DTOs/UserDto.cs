using ReForm.Core.Models.Identity;

namespace ReForm.Core.DTOs;

public class UserDto
{
    public int Id { get; init; }

    public string Name { get; init; }

    public string Email { get; init; }

    public DateTime? LastLogin { get; set; }

    public DateTime RegistrationTime { get; init; }

    public bool IsBlocked { get; set; }

    public bool IsAdmin { get; init; }

    public UserDto(User user) : this(user, false) {}

    public UserDto(User user, bool isAdmin)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email!;
        LastLogin = user.LastLogin;
        RegistrationTime = user.RegistrationTime;
        IsBlocked = user.IsBlocked;
        IsAdmin = isAdmin;
    }
}