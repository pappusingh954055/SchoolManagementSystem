using School.Application.Interfaces;

namespace School.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly SchoolDbContext _context;

    public UnitOfWork(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
