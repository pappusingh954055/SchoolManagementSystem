using Microsoft.EntityFrameworkCore;
using Student.Application.Interfaces;
using Student.Domain.Entities;
using Student.Infrastructure.Persistence;

namespace Student.Infrastructure.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly StudentDbContext _context;

    public StudentRepository(StudentDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Student.Domain.Entities.Student student)
    {
        await _context.Students.AddAsync(student);
    }

    public async Task<Student.Domain.Entities.Student?> GetByIdAsync(Guid id)
    {
        return await _context.Students
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> ExistsByStudentCodeAsync(string studentCode)
    {
        return await _context.Students
            .AnyAsync(x => x.StudentCode == studentCode);
    }

    public void Remove(Student.Domain.Entities.Student student)
    {
        _context.Students.Remove(student);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
