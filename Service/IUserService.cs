using OnlineShopAPI.Entity;

namespace OnlineShopAPI.Service;

public interface IUserService
{
    void AddUser(User user);

    User? GetUserByUserName(string username);

    IList<User> GetAllUsers();

    void DeleteUser(User user);
    public void DeleteUserByUsername(String username);
}