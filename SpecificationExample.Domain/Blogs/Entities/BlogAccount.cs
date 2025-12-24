using SpecificationExample.Domain.Common;

namespace SpecificationExample.Domain.Blogs;

public class BlogAccount : Entity
{
    public string Name { get; set; } = string.Empty;
    public IEnumerable<Blog> Blogs { get; set; } = new List<Blog>();

    public BlogAddress AccountAddress { get; set; } = null!;
    public Autor Autor { get; set; } = null!;
}
