using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using ReForm.Core.Models.Identity;

namespace ReForm.Presentation.Controllers;

[Authorize]
[Route("api/user")]
public class UserController(UserManager<User> userManager, IUserService userService) : Controller
{
    [HttpGet]
    [IgnoreAntiforgeryToken]
    [Route("index")]
    public async Task<IActionResult> Index()
    {
        var currentUser = await userManager.GetUserAsync(User);
        if (currentUser == null || currentUser.IsBlocked)
        {
            return Unauthorized();
        }

        var users = await userService.GetAllUsersAsync();
        return View(users);
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    [Route("block")]
    public async Task<IActionResult> BlockUsers([FromBody] List<int> userIds)
    {
        var currentUser = await userManager.GetUserAsync(User);
        if (currentUser == null || currentUser.IsBlocked)
        {
            return Unauthorized();
        }

        await userService.BlockUsersAsync(userIds);
        return Ok();
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    [Route("unblock")]
    public async Task<IActionResult> UnblockUsers([FromBody] List<int> userIds)
    {
        var currentUser = await userManager.GetUserAsync(User);
        if (currentUser == null || currentUser.IsBlocked)
        {
            return Unauthorized();
        }

        await userService.UnblockUsersAsync(userIds);
        return Ok();
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    [Route("delete")]
    public async Task<IActionResult> DeleteUsers([FromBody] List<int> userIds)
    {
        var currentUser = await userManager.GetUserAsync(User);
        if (currentUser == null || currentUser.IsBlocked)
        {
            return Unauthorized();
        }

        await userService.DeleteUsersAsync(userIds);
        return Ok();
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    [Route("change-role")]
    public async Task<IActionResult> ChangeUsersRole([FromBody] List<int> userIds)
    {
        var currentUser = await userManager.GetUserAsync(User);
        if (currentUser == null || currentUser.IsBlocked)
        {
            return Unauthorized();
        }

        await userService.ChangeUsersRole(userIds);
        return Ok();
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query required");

        var matches = await userService
            .SearchUsersAsync(query);

        return Ok(matches);
    }

    [HttpGet("getAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await userService.GetAllUsersAsync();
        return Ok(users);
    }
}