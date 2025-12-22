

using SpecificationExample.Domain.Common;

namespace SpecificationExample.Domain.Blogs;

public class BlogWithPostsSpecification : Specification<Blog>
{
    public BlogWithPostsSpecification()
    {
        Rule(blog => blog.Posts.Any());
    }
}
