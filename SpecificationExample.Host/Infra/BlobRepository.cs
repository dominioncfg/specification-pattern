using Microsoft.EntityFrameworkCore;
using SpecificationExample.Domain.Blogs;
using SpecificationExample.Domain.Common;

namespace SpecificationExample.Infra;

public class BlogAccountRepository : IBlogAccountRepository
{
    private readonly AppDbContext _context;
    public BlogAccountRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task Add(BlogAccount entity, CancellationToken cancellation)
    {
        await _context.Accounts.AddAsync(entity, cancellation);
        await _context.SaveChangesAsync();
    }
    public async Task<BlogAccount?> GetById(int id, CancellationToken cancellation)
    {
        return await _context.Accounts.FindAsync(id);
    }

    public async Task Delete(BlogAccount entity, CancellationToken cancellation)
    {
        _context.Accounts.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<BlogAccount>> Filter(Specification<BlogAccount> filter, CancellationToken cancellation)
    {
        return (await _context.Accounts.FilterWithEfCore(filter).ToListAsync(cancellation)).AsEnumerable();
    }

    public async Task<BlogAccount> Get(Specification<BlogAccount> filter, CancellationToken cancellationToken)
    {
        return await _context.Accounts.FilterWithEfCore(filter).FirstAsync(cancellationToken);
    }
}