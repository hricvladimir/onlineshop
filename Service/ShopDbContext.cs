using Microsoft.EntityFrameworkCore;
using OnlineShopAPI.Entity;


namespace OnlineShopAPI.Service;




public class ShopDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL($"Server=127.0.0.1;Database={Constants.Database};User={Constants.User};Password={Constants.Password};Port={Constants.Port};");
    }
}
