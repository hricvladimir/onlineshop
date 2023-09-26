using Microsoft.AspNetCore.Mvc;
using OnlineShopAPI.Entity;
using OnlineShopAPI.Service;

namespace OnlineShopAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _service = new();
    
    // --- GET ALL USERS ---
    [HttpGet(Name = "GetUsers")]
    public IList<User> Get()
    {
        return _service.GetAllUsers();
    }
    
    // --- CREATE NEW USER ---
    [HttpPost(Name = "CreateUser")]
    public IActionResult CreateUser([FromBody] User user)
    {
        User? duplicateUser = _service.GetUserByUserName(user.username);
        if (!ModelState.IsValid)
            return BadRequest("Invalid user data.");
        if (duplicateUser != null)
            return BadRequest("User with this username already exists.");
        
        _service.AddUser(user);
        return Ok("User created successfully.");
    }

    // DELETE USER
    [HttpDelete(Name = "DeleteUser")]
    public IActionResult DeleteUser([FromBody] String username)
    {
        User? duplicateUser = _service.GetUserByUserName(username);
        if (!ModelState.IsValid)
            return BadRequest("Invalid user data.");
        if (duplicateUser == null)
        {
            return BadRequest("User with this username does not exist.");
        }
        
        _service.DeleteUserByUsername(username);
        return Ok($"User {username} was deleted successfully.");
    }
}