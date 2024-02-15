using InventoryManager.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace InventoryManager.Data
{
    internal class InventoryContext : DbContext
    {
        public InventoryContext() { }

        public InventoryContext(DbContextOptions options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<InventoryEntry> Inventory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var connectionString = File.ReadAllText($"{assemblyLocation}/ConnectionString.txt");
            var serverVersion = new MySqlServerVersion(ServerVersion.AutoDetect(connectionString));
            optionsBuilder.UseLazyLoadingProxies()
                .UseMySql(connectionString, serverVersion);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Navigation(e => e.Category).AutoInclude();
            modelBuilder.Entity<Warehouse>().Navigation(e => e.Location).AutoInclude();
            modelBuilder.Entity<InventoryEntry>().Navigation(e => e.Product).AutoInclude();
            modelBuilder.Entity<InventoryEntry>().Navigation(e => e.Warehouse).AutoInclude();
        }
    }
}
