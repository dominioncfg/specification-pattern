using Microsoft.EntityFrameworkCore;
using SpecificationExample.Domain.Blogs;
using SpecificationExample.Domain.Common;

namespace SpecificationExample.Infra;

public class BlobRepository : IBlogRepository
{
    private readonly AppDbContext _context;
    public BlobRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task Add(Blog entity, CancellationToken cancellation)
    {
        await _context.Blogs.AddAsync(entity, cancellation);
        await _context.SaveChangesAsync();
    }
    public async Task<Blog?> GetById(int id, CancellationToken cancellation)
    {
        return await _context.Blogs.FindAsync(id);
    }

    public async Task Delete(Blog entity, CancellationToken cancellation)
    {
        _context.Blogs.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Blog>> Filter(Specification<Blog> filter, CancellationToken cancellation)
    {
        return (await _context.Blogs.FilterWithEfCore(filter).ToListAsync(cancellation)).AsEnumerable();
    }

    public async Task<Blog> Get(Specification<Blog> filter, CancellationToken cancellationToken)
    {
        return await _context.Blogs.FilterWithEfCore(filter).FirstAsync(cancellationToken);
    }
}