using BlogDbLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogDbLibrary
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions options) : base(options)
        {
        }

        public BlogDbContext()
        {
        }

        public DbSet<Post> Posts { get; set; }
    }
}