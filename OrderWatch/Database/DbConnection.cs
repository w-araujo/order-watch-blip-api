using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using OrderWatch.Models;

namespace OrderWatch.Database
{
    public class DbConnection : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products {  get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        public string DbPath { get; }

        public DbConnection(DbContextOptions<DbConnection> options) : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "orderWatch.db");
            Console.WriteLine($"local banco de dados: {DbPath}");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasMany(e => e.Products)
            .WithMany(e => e.Users)
            .UsingEntity<Purchase>();

            modelBuilder.Entity<User>()
              .HasIndex(u => u.Email)
              .IsUnique();
        }
    }
}
