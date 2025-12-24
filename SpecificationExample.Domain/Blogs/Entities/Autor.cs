using SpecificationExample.Domain.Common;

namespace SpecificationExample.Domain.Blogs;

public class Autor : Entity
{
    public string FullName { get; set; } = string.Empty;
    public AutorAge Age { get; set; } = null!;
}
