using School.Domain.Entities;

namespace School.Application.Interfaces;

public interface ISchoolRepository
{
    Task AddAsync(School.Domain.Entities.School school);
    Task<School.Domain.Entities.School?> GetByIdAsync(Guid id);
    Task<bool> ExistsByCodeAsync(string code);
    Task SaveChangesAsync();
}
