namespace OnlineShopAPI.Entity;

[Serializable]
public class User
{
    public int Id { get; set; }
    public string username { get; set; }
    public string hashedPassword { get; set; }
    public string email { get; set; }
    public string salt { get; set; }
}