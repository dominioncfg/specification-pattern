using SpecificationTests.Domain.Common;

namespace SpecificationTests.Domain.Blogs;

public class Post: Entity
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int BlogId { get; set; }
    public Blog Blog { get; set; } = null!;
}
