
using SpecificationTests.Domain.Common;

namespace SpecificationTests.Domain.Blogs;

public class BlogWithPostsSpecification : Specification<Blog>
{
    public BlogWithPostsSpecification()
    {
        Rule(blog => blog.Posts.Any());
    }
}
