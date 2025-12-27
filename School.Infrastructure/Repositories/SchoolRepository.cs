using Microsoft.EntityFrameworkCore;
using School.Application.Interfaces;
using School.Infrastructure.Persistence;

namespace School.Infrastructure.Repositories;

public class SchoolRepository : ISchoolRepository
{
    private readonly SchoolDbContext _context;

    public SchoolRepository(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(School.Domain.Entities.School school)
    {
        await _context.Schools.AddAsync(school);
    }

    public async Task<School.Domain.Entities.School?> GetByIdAsync(Guid id)
    {
        return await _context.Schools
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<bool> ExistsByCodeAsync(string code)
    {
        return await _context.Schools
            .AnyAsync(s => s.Code.Value == code);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
