using Microsoft.AspNetCore.Mvc;
using OnlineShopAPI.Entity;
using OnlineShopAPI.Service;

namespace OnlineShopAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _service = new();

    [HttpGet(Name = "GetUsers")]
    public IList<User> Get()
    {
        return _service.GetAllUsers();
    }

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