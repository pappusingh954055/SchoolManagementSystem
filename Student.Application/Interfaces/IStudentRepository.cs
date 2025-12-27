using Student.Domain.Entities;

namespace Student.Application.Interfaces;

public interface IStudentRepository
{
    Task AddAsync(Student.Domain.Entities.Student student);
    Task<Student.Domain.Entities.Student?> GetByIdAsync(Guid id);
    Task<bool> ExistsByStudentCodeAsync(string studentCode);
    void Remove(Student.Domain.Entities.Student student);
    Task SaveChangesAsync();
}
