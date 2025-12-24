using SpecificationExample.Domain.Common;

namespace SpecificationExample.Domain.Blogs;

public class BlogCategory : Entity
{
    public string Name { get; set; } = string.Empty;

    public IEnumerable<Post> Posts { get; set; } = new List<Post>();
}