using SpecificationTests.Domain.Common;

namespace SpecificationTests.Domain.Blogs;

public class Blog : Entity
{
    public string Name { get; set; } = string.Empty;
    public List<Post> Posts { get; set; } = new();
}
