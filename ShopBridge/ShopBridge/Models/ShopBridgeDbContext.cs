using Microsoft.EntityFrameworkCore;
using ShopBridge.Entities;

namespace ShopBridge.Models
{
    public class ShopBridgeDbContext : DbContext
    {
        public ShopBridgeDbContext(DbContextOptions<ShopBridgeDbContext> options) : base(options)
        {
        }

        public DbSet<Inventory> Inventories { get; set; }
    }
}
