using Microsoft.EntityFrameworkCore;
using School.Application.Interfaces;
using School.Infrastructure.Persistence;
using School.Domain.Entities;

namespace School.Infrastructure.Repositories;

public class SchoolRepository : ISchoolRepository
{
    private readonly SchoolDbContext _context;

    public SchoolRepository(SchoolDbContext context)
    {
        _context = context;
    }

    // ---------------- ADD ----------------
    public async Task AddAsync(School.Domain.Entities.School school)
    {
        await _context.Schools.AddAsync(school);
    }

    // ---------------- GET BY ID (TRACKED) ----------------
    public async Task<Domain.Entities.School?> GetByIdAsync(Guid id)
    {
        return await _context.Schools
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    // ---------------- EXISTS BY CODE ----------------
    public async Task<bool> ExistsByCodeAsync(string code)
    {
        return await _context.Schools
            .AnyAsync(s => s.Code == code);
    }

    // ---------------- REMOVE ----------------
    public void Remove(Domain.Entities.School school)
    {
        _context.Schools.Remove(school);
    }

    // ---------------- SAVE ----------------
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
