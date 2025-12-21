
using SpecificationTests.Domain.Common;

namespace SpecificationTests.Domain.Blogs;

public class FirstTenBlogByNameSpecification : Specification<Blog>
{
    public FirstTenBlogByNameSpecification(string name)
    {
        Rule(query => name == query.Name)
            .OrderBy(blog => blog.Name)
            .Paginate(0, 10)
            .Include(blog => blog.Posts)
            .AsSplitQuery()
            .QueryTag($"This is the {nameof(FirstTenBlogByNameSpecification)} query");
    }
}
