using ContactManager.Data;
using ContactManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ContactDbContext _context;
        private IGenericRepository<User> _users;

        public UnitOfWork(ContactDbContext context)
        {
            _context = context;
        }
        public IGenericRepository<User> Users => _users ??= new GenericRepository<User>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
