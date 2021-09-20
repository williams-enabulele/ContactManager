using ContactManager.Model;
using System;
using System.Threading.Tasks;

namespace ContactManager.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> Users { get; }

        Task Save();
    }
}