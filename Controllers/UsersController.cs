using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        return users.Count > 0 ? Ok(users) : NotFound("No users found."); 
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
    public IActionResult CreateUser([FromQuery] String username, [FromQuery] String password, [FromQuery] String email)
    {

        if (username == "" || password == "" || email == "")
        {
            return BadRequest("NULL");
        }
        User? duplicateUser = _service.GetUserByUserName(username);

        if (duplicateUser != null)
            return BadRequest("User with this username already exists.");
        
        // creating new user
        User newUser = new User();
        
        // username and email
        newUser.username = username;
        newUser.email = email;
        
        // creating a random salt
        byte[] salt = getSalt();
        newUser.salt = Convert.ToBase64String(salt);
        
        // salting and hashing password
        byte[] hashBytes = GenerateSaltedHash(Encoding.ASCII.GetBytes(password), salt);
        string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        newUser.hashedPassword = hashString;
        
        // adding user to DB
        _service.AddUser(newUser);
        return Ok("User created successfully.");
    }
    
    static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
    {
        HashAlgorithm algorithm = new SHA256Managed();

        byte[] plainTextWithSaltBytes = 
            new byte[plainText.Length + salt.Length];

        for (int i = 0; i < plainText.Length; i++)
        {
            plainTextWithSaltBytes[i] = plainText[i];
        }
        for (int i = 0; i < salt.Length; i++)
        {
            plainTextWithSaltBytes[plainText.Length + i] = salt[i];
        }

        return algorithm.ComputeHash(plainTextWithSaltBytes);            
    }
    
    private byte[] getSalt()
    {
        var random = new RNGCryptoServiceProvider();

        // Maximum length of salt
        int maxLength = 32;

        // Empty salt array
        byte[] salt = new byte[maxLength];

        // Build the random bytes
        random.GetNonZeroBytes(salt);

        // Return the string encoded salt
        return salt;
    }
    
    //--- UPDATE USER ---
    [HttpPut("update/{id:int}", Name = "UpdateUser")]
    public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
    {
        User? userToUpdate = _service.GetUserById(id);
        if (userToUpdate == null)
        {
            return BadRequest("No user to update was found.");
        }
    
        _service.UpdateUser(updatedUser);
        return Ok("User was updated");
    }

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