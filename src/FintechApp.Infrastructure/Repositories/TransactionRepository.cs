using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FintechApp.Application.DTOs;
using FintechApp.Domain.Entities;
using FintechApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FintechApp.Infrastructure.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<Transaction>> GetByWalletIdAsync(int walletId)
        {
            return await _context.Transactions
                .Where(t => t.FromWalletId == walletId || t.ToWalletId == walletId)
                .Include(t => t.FromWallet)
                .Include(t => t.ToWallet)
            .ToListAsync();
        }

        public async Task<int> CountByWalletIdAsync(int walletId)
        {
            return await _context.Transactions
                .CountAsync(t => t.FromWalletId == walletId || t.ToWalletId == walletId);
        }

        public async Task<List<Transaction>> GetPagedByWalletAsync(int walletId, int pageNumber, int pageSize)
        {
            var query = _dbSet
                .Include(t => t.FromWallet).ThenInclude(w => w.Currency)
                .Include(t => t.ToWallet).ThenInclude(w => w.Currency)
                .Where(t => t.FromWalletId == walletId || t.ToWalletId == walletId);

            return await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task<List<Transaction>> SearchAsync(DateTime? fromDate, DateTime? toDate, int? walletId, int pageNumber, int pageSize)
        {
            var query = _dbSet
             .Include(t => t.FromWallet).ThenInclude(w => w.Currency)
             .Include(t => t.ToWallet).ThenInclude(w => w.Currency)
             .Where(t => t.FromWalletId == walletId || t.ToWalletId == walletId);



            if (fromDate.HasValue)
                query = query.Where(t => t.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(t => t.CreatedAt <= toDate.Value);

            if (walletId.HasValue)
                query = query.Where(t => t.FromWalletId == walletId.Value || t.ToWalletId == walletId.Value);

            return await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountSearchAsync(DateTime? fromDate, DateTime? toDate, int? walletId)
        {
            var query = _dbSet.AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(t => t.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(t => t.CreatedAt <= toDate.Value);

            if (walletId.HasValue)
                query = query.Where(t => t.FromWalletId == walletId.Value || t.ToWalletId == walletId.Value);

            return await query.CountAsync();
        }

    }
}
