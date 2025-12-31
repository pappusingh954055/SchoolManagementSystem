using School.Domain.Entities;

namespace School.Application.Interfaces;

public interface ISchoolRepository
{
    Task AddAsync(Domain.Entities.School school );
    Task<Domain.Entities.School?> GetByIdAsync(Guid id);
    Task<bool> ExistsByCodeAsync(string code);
    void Remove(Domain.Entities.School school);
}
