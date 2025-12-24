using SpecificationExample.Domain.Blogs;
using SpecificationExample.Domain.Common;
using SpecificationExample.Infra;
Seed();


var filter = new BlogAccountAggregateSpecification();


//Test EFCore 
using var db = new AppDbContext();
IBlogAccountRepository blogRepository = new BlogAccountRepository(db);

var blogAccounts = await blogRepository.Filter(filter, default);

if (!blogAccounts.Any())
{
    Console.WriteLine("[EF Core] - Blog not found");
}
else
{
    foreach (var blog in blogAccounts)
    {
        Console.WriteLine($"[EF Core] - Blog Account - {blog.Name}");
    }
}

//Test InMemory
var blogsInMemory = SeedInMemory();
var filteredBlogAccounts = blogsInMemory.Filter(filter);

if (!filteredBlogAccounts.Any())
{
    Console.WriteLine("[In Memory] - Blog not found");
}
else
{
    foreach (var blog in blogAccounts)
    {
        Console.WriteLine($"[In Memory] - Blog Account - {blog.Name}");
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
        db.Accounts.Add(item);
    }
    db.SaveChanges();
}

static List<BlogAccount> SeedInMemory()
{
    var commonCategory = new BlogCategory
    {
        Id = 1,
        Name = "Programming"
    };
    return
    [
        new ()
        {
            Id =1,
            Name = "First Blog Account",
            AccountAddress = new BlogAddress
            {
                Id = 4,
                Street = "789 Corporate Blvd",
                City = "Business City",
                State = "TX",
                ZipCode = "75001"
            },
            Autor =  new Autor
            {
                Id = 1,
                FullName = "John Doe",
                Age =  new AutorAge
                {
                    Id = 1,
                    Age = 30
                }
            },

            Blogs  = [
                new Blog()
                {
                    Id = 1,
                    Name = "My Tech Blog",
                    BlogAddress = new BlogAddress
                    {
                        Id = 5,
                        Street = "321 Tech Ave",
                        City = "Innovate Town",
                        State = "WA",
                        ZipCode = "98001"
                    },
                    Posts = [
                        new Post
                        {
                            Id = 1,
                            Title = "EF Core Basics",
                            Content = "Learning EF Core...",
                            Address = new BlogAddress
                            {
                                Id = 1,
                                Street = "123 Main St",
                                City = "Techville",
                                State = "CA",
                                ZipCode = "90001",
                            },
                             Categories =
                             [
                                commonCategory,
                                new BlogCategory
                                {
                                    Id = 3,
                                    Name = "EF Core"
                                }
                             ]
                        },
                        new Post
                        {
                            Id = 2,
                            Title = "C# Tips",
                            Content = "Some useful C# tips...",
                            Address = new BlogAddress
                            {
                                Id = 2,
                                Street = "456 Side St",
                                City = "Code City",
                                State = "NY",
                                ZipCode = "10001"
                            },
                            Categories = [
                                commonCategory,
                                new BlogCategory
                                {
                                    Id = 2,
                                    Name = "C#"
                                }
                            ]
                        }
                    ]
                }],
        }
    ];
}