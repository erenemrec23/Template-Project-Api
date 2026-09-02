using QrAssignment.Application.Abstractions;
using QrAssignment.Persistance.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Persistance.Repositories
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
