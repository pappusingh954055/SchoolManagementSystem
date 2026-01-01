using Employee.Domain.Entities;

namespace Employee.Application.Interfaces;

public interface IEmployeeRepository
{
    Task AddAsync(Domain.Entities.Employee employee);
    Task<Domain.Entities.Employee?> GetByIdAsync(Guid id);
    Task<List<Domain.Entities.Employee>> GetAllAsync();
    void Remove(Domain.Entities.Employee employee);
}
