using btlAspnet.Models.DataModels;
using Microsoft.EntityFrameworkCore;

namespace btlAspnet.Models.BusinessModels
{
    public class ShoppingContext : DbContext
    {
        public ShoppingContext() { }

        public ShoppingContext(DbContextOptions<ShoppingContext> options) : base(options)
        {

        }

        // Khai báo tập thực thể:
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
