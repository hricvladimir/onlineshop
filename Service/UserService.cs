using Microsoft.EntityFrameworkCore;
using OnlineShopAPI.Entity;

namespace OnlineShopAPI.Service;

public class UserService : IUserService
{
    private ShopDbContext _context = new();
    
    
    public void AddUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public User? GetUserByUserName(string username)
    {
        return _context.Users.FirstOrDefault(s => s.username == username);
    }

    public IList<User> GetAllUsers()
    {
        return _context.Users.ToArray();
    }

    public void DeleteUser(User user)
    {
        _context.Users.Remove(user);
        _context.SaveChanges();
    }

    public void DeleteUserByUsername(String username)
    {
        User? user = GetUserByUserName(username);
        Console.WriteLine("kasta");
        if (user != null)
        {
            Console.WriteLine("kasta");
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
            
    }
}