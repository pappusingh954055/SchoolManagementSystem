using Employee.Application.Interfaces;
using Employee.Domain.Entities;
using Employee.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Employee.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly EmployeeDbContext _context;

    public EmployeeRepository(EmployeeDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Domain.Entities.Employee employee)
    {
        await _context.Employees.AddAsync(employee);
    }

    public async Task<Domain.Entities.Employee?> GetByIdAsync(Guid id)
    {
        return await _context.Employees.FindAsync(id);
    }

    public async Task<List<Domain.Entities.Employee>> GetAllAsync()
    {
        return await _context.Employees.ToListAsync();
    }

    public void Remove(Domain.Entities.Employee employee)
    {
        _context.Employees.Remove(employee);
    }
}
