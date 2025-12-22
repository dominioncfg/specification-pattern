using SpecificationExample.Domain.Blogs;
using SpecificationExample.Domain.Common;
using SpecificationExample.Infra;
Seed();


var filter = new FirstTenBlogByNameSpecification("My Tech Blog")
    .And(new BlogWithPostsSpecification());


//Test EFCore 
using var db = new AppDbContext();
IBlogRepository blogRepository = new BlobRepository(db);

var blogs = await blogRepository.Filter(filter, default);

if (!blogs.Any())
{
    Console.WriteLine("[EF Core] - Blog not found");
}
else
{
    foreach (var blog in blogs)
    {
        Console.WriteLine($"[EF Core] - Blog found - {blog.Name}");
    }
}

//Test InMemory
var blogsInMemory = SeedInMemory();
var filteredBlog = blogsInMemory.Filter(filter);

if (!filteredBlog.Any())
{
    Console.WriteLine("[In Memory] - Blog not found");
}
else
{
    foreach (var blog in blogs)
    {
        Console.WriteLine($"[In Memory] - Blog found - {blog.Name}");
    }
}



static void Seed()
{
    using var db = new AppDbContext();

    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();


    var blogs = SeedInMemory();
    foreach (var item in blogs)
    {
        db.Blogs.Add(item); 
    }
    db.SaveChanges();
}

static List<Blog> SeedInMemory()
{
    return
    [
        new()
        {
            Id = 1,
            Name = "My Tech Blog",
            Posts = [
                new Post { Id = 1, Title = "EF Core Basics", Content = "Learning EF Core..." },
                new Post { Id = 2, Title = "C# Tips", Content = "Some useful C# tips..." }
            ]
        }
    ];
}