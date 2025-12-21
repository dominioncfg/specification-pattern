using Microsoft.EntityFrameworkCore;
using SpecificationTests.Domain.Blogs;

namespace SpecificationTests.Infra;

public class AppDbContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
          .UseSqlite("Data Source=app.db")
          .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
          .EnableSensitiveDataLogging()
          .EnableDetailedErrors();
    }
}
