using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using GameDevHelper.Models;
using GameDevHelperOpenApi.DTO;
using GameDevHelperOpenApi.Services;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly UserSearchService _userSearchService;

    public UserController(UserManager<User> userManager, UserSearchService userSearchService)
    {
        _userManager = userManager;
        _userSearchService = userSearchService;
    }

    /// <summary>
    /// Registers a user using the DTO model
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    //api/users/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new User
        {
            UserName = model.Username,
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(new { Message = "User registered successfully", UserId = user.Id });
    }


    /// <summary>
    /// Returns all users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = _userManager.Users;
        return Ok(users);
    }

    /// <summary>
    /// Search user by userName or eMail or id
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="eMail"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("search")]
    public async Task<IEnumerable<User>> SearchUser(string? userName, string? eMail, string? id)
    {
        var result = await _userSearchService.SearchUser(userName,eMail,id);
        return result;
    }


    /// <summary>
    /// Delete user by username
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    [HttpDelete("{userName}/delete")]
    public async Task<IActionResult> DeleteUser(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);

        if (user == null)
        { 
            return NotFound();
        }

        await _userManager.DeleteAsync(user);
        return Ok(user);
    }

    /// <summary>
    /// Update/Change username
    /// </summary>
    /// <param name="name"></param>
    /// <param name="userName"></param>
    /// <returns></returns>
    [HttpPatch("{Name}/{userName}/updateUserName")]
    public async Task<IActionResult> UpdateUserName(string name, string userName)
    {
        var user = await _userManager.FindByNameAsync(name);

        if (user == null)
        {
            return NotFound();
        }
        else
        {
            user.UserName = userName;
        }

        await _userManager.UpdateAsync(user);
        return Ok(userName);
    }
}
