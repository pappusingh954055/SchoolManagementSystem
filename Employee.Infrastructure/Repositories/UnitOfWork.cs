using Employee.Application.Interfaces;
using Employee.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Employee.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EmployeeDbContext _context;

        public UnitOfWork(EmployeeDbContext context)
        {
            _context = context;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

}
