using Microsoft.AspNetCore.Mvc;
using OnlineShopAPI.Entity;
using OnlineShopAPI.Service;

namespace OnlineShopAPI.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UsersController : ControllerBase
{
    private readonly UserService _service = new();
    
    // --- GET ALL USERS ---
    [HttpGet(Name = "GetAllUsers")]
    public IActionResult GetUsers()
    {
        IList<User> users = _service.GetAllUsers();
        return users.Count == 0 ? Ok(users) : NotFound("No users found."); 
    }
    
    // --- GET USER BY USERNAME
    [HttpGet("{username}", Name = "GetUserByUserName")]
    public IActionResult GetUser(String username)
    {
        User? user = _service.GetUserByUserName(username);
        return user != null ? Ok(user) : NotFound("User not found.");
    }
    
    
    // --- CREATE NEW USER ---
    [HttpPost("create", Name = "CreateUser")]
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
    
    // --- UPDATE USER ---
    // TODO
    // public IActionResult UpdateUser([FromBody] User updatedUser)
    // {
    //     User? userToUpdate = _service.GetUserById(updatedUser.Id);
    //     if (userToUpdate == null)
    //     {
    //         return BadRequest("No user to update was found.");
    //     }
    //
    //     userToUpdate.username = updatedUser.username;
    // }

    // DELETE USER
    [HttpDelete("delete/{username}")]
    public IActionResult DeleteUser(String username)
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