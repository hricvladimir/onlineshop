using Microsoft.EntityFrameworkCore;
using OnlineShopAPI.Entity;


namespace OnlineShopAPI.Service;


public class ShopDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // create configuration from appsettings.json
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        // obtain connection string from configuration
        string connectionString = configuration.GetConnectionString("Default") ?? throw new ArgumentException("Connection string was not loaded.");
        
        // start sql server
        optionsBuilder.UseMySQL(connectionString);
    }
}
