using Microsoft.EntityFrameworkCore;
using SpecificationExample.Domain.Blogs;

namespace SpecificationExample.Infra;

public class AppDbContext : DbContext
{
    public DbSet<BlogAccount> Accounts { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<BlogAddress> Addresses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
          .UseSqlite("Data Source=app.db")
          .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
          .EnableSensitiveDataLogging()
          .EnableDetailedErrors();
    }
}
