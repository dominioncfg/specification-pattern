
using SpecificationExample.Domain.Common;

namespace SpecificationExample.Domain.Blogs;

public class BlogAccountByNameSpecification : DomainSpecification<BlogAccount>
{
    public BlogAccountByNameSpecification(string name)
    {
        Rule(query => name == query.Name);
    }
}
