
using SpecificationExample.Domain.Common;

namespace SpecificationExample.Domain.Blogs;

public class BlogAccountAggregateSpecification : Specification<BlogAccount>
{
    public BlogAccountAggregateSpecification()
    {
        // First chain: BlogAccount -> Blogs -> Posts -> Address

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

        //// Second chain: BlogAccount -> Blogs -> Posts -> Categories
        //Include(account => account.Blogs)
        //    .ThenInclude(blog => blog.Posts)
        //    .ThenInclude(post => post.Categories);
        //.ThenInclude(post => post.a);
        //ThenInclude<Post, BlogAddress>(x => x.Address);
        //ThenInclude<Post, BlogCategory>(x => x.Categories);
        AsSplitQuery();
    }

}
