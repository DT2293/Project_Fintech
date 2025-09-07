using System;
using System.Threading.Tasks;
using FintechApp.Domain.Entities;
using FintechApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace FintechApp.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed = false;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Roles = new RoleRepository(_context);
            UserWallets = new UserWalletRepository(_context);
            Transactions = new TransactionRepository(_context);
            TransactionEntries = new TransactionEntryRepository(_context);
        }

        public IUserRepository Users { get; }
        public IRoleRepository Roles { get; }

        public IUserWalletRepository UserWallets { get; }
        public ITransactionRepository Transactions { get; }

        public ITransactionEntryRepository TransactionEntries { get; } 

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
