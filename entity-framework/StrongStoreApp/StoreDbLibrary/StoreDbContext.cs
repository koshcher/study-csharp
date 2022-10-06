using Microsoft.EntityFrameworkCore;
using StoreDbLibrary.Models;

namespace StoreDbLibrary
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext()
        {
        }

        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {
        }

        public DbSet<AppCategory> AppCategories { get; set; }

        public DbSet<AppLanguage> AppLanguages { get; set; }

        public DbSet<App> Apps { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=sqland.database.windows.net;Initial Catalog=StoreDb;Persist Security Info=True;User ID=sqlander;Password=5ql@nder");
            }
        }
    }
}