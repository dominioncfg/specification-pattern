using SpecificationExample.Domain.Blogs;
using SpecificationExample.Domain.Common;
using SpecificationExample.Infra;
Seed();


//Test EFCore Query Specification
var filter = new BlogAccountAggregateQuerySpecification();
using var db = new AppDbContext();
IBlogAccountRepository blogRepository = new BlogAccountRepository(db);

var blogAccounts = await blogRepository.Filter(filter, default);

if (!blogAccounts.Any())
{
    Console.WriteLine("[EF Core] - Blog Account not found");
}
else
{
    foreach (var blog in blogAccounts)
    {
        Console.WriteLine($"[EF Core] - Blog Account - {blog.Name}");
    }
}

//Test InMemory Domain Specification
var blogsInMemory = SeedInMemory();
var domainQuery = new BlogAccountByNameSpecification("First Blog Account");
var filteredBlogAccounts = blogsInMemory.Filter(domainQuery);

if (!filteredBlogAccounts.Any())
{
    Console.WriteLine("[In Memory] - Blog Account not found");
}
else
{
    foreach (var blog in blogAccounts)
    {
        Console.WriteLine($"[In Memory] - Blog Account - {blog.Name}");
    }
}
//==============================================


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
        Id = Guid.Parse("96cfd70a-e1d0-4a39-a636-0f091c1837a1"),
        Name = "Programming"
    };
    return
    [
        new ()
        {
            Id = Guid.Parse("37002a2a-6a74-4629-9817-7af3a03b812f"),
            Name = "First Blog Account",
            AccountAddress = new BlogAddress
            {
                Id = Guid.Parse("eac038a1-f8e2-43b1-a417-19f0cad61065"),
                Street = "789 Corporate Blvd",
                City = "Business City",
                State = "TX",
                ZipCode = "75001"
            },
            Autor =  new Autor
            {
                Id = Guid.Parse("27cfb9b4-de65-44e7-aed1-42de48668876"),
                FullName = "John Doe",
                Age =  new AutorAge
                {
                    Id = Guid.Parse("f8769d18-04c3-4523-aa45-a873b1adc2b0"),
                    Age = 30
                }
            },

            Blogs  = [
                new Blog()
                {
                    Id = Guid.Parse("527e42e2-b591-4a40-a8bf-1179981b6330"),
                    Name = "My Tech Blog",
                    BlogAddress = new BlogAddress
                    {
                        Id = Guid.Parse("c2ff44ea-d842-41c8-aab9-ca2f76b3971a"),
                        Street = "321 Tech Ave",
                        City = "Innovate Town",
                        State = "WA",
                        ZipCode = "98001"
                    },
                    Posts = [
                        new Post
                        {
                            Id = Guid.Parse("108fcfee-4fe1-428b-87a3-0deac469bd46"),
                            Title = "EF Core Basics",
                            Content = "Learning EF Core...",
                            Address = new BlogAddress
                            {
                                Id = Guid.Parse("a68b7bc1-303a-4b3a-b07a-6cc456163855"),
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
                                    Id = Guid.Parse("df93982b-0dce-463e-994b-061bdf46b8c2"),
                                    Name = "EF Core"
                                }
                             ]
                        },
                        new Post
                        {
                            Id = Guid.Parse("ea931eba-8825-44c3-acff-d8134f731b31"),
                            Title = "C# Tips",
                            Content = "Some useful C# tips...",
                            Address = new BlogAddress
                            {
                                Id = Guid.Parse("3d88eabe-831e-4a2d-94ff-3ca8371f72a2"),
                                Street = "456 Side St",
                                City = "Code City",
                                State = "NY",
                                ZipCode = "10001"
                            },
                            Categories = [
                                commonCategory,
                                new BlogCategory
                                {
                                    Id = Guid.Parse("bc8436ff-dd84-489f-881a-e8543d6e5344"),
                                    Name = "C#"
                                }
                            ]
                        }
                    ]
                }],
        }
    ];
}