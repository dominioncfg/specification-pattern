
using SpecificationExample.Domain.Common;

namespace SpecificationExample.Domain.Blogs;

public class FirstTenBlogByNameSpecification : Specification<Blog>
{
    public FirstTenBlogByNameSpecification(string name)
    {
        Rule(query => name == query.Name)
            .OrderBy(blog => blog.Name)
            .AsSplitQuery()
            .QueryTag($"This is the {nameof(FirstTenBlogByNameSpecification)} query")
            .Paginate(0, 10)
            .Include(blog => blog.Posts);
    }
}
