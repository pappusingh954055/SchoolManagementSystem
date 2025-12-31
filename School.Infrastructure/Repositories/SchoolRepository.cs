using Microsoft.EntityFrameworkCore;
using School.Application.Interfaces;
using School.Infrastructure.Persistence;

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
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> ExistsByCodeAsync(string code)
    {
        return await _context.Schools
            .AnyAsync(x => x.Code == code);
    }

    public void Remove(School.Domain.Entities.School school)
    {
        _context.Schools.Remove(school);
    }
}
