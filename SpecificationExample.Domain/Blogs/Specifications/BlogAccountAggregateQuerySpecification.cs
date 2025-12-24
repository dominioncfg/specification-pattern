
using SpecificationExample.Domain.Common;

namespace SpecificationExample.Domain.Blogs;

public class BlogAccountAggregateQuerySpecification : QuerySpecification<BlogAccount>
{
    public BlogAccountAggregateQuerySpecification()
    {
        this
          .Include(account => account.AccountAddress)
          .Include(account => account.Blogs)
          .ThenInclude(blog => blog.Posts)
          .ThenInclude(post => post.Categories);

        this
          .Include(account => account.Blogs)
          .ThenInclude(blog => blog.BlogAddress);


        this
          .Include(account => account.Blogs)
          .ThenInclude(blog => blog.Posts)
          .ThenInclude(post => post.Address);

        this.Include(account => account.Autor)
            .ThenInclude(autor => autor.Age);

        AsSplitQuery();
    }

}
