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
            return await _context.Transactions
                .Where(t => t.FromWalletId == walletId || t.ToWalletId == walletId)
                .Include(t => t.FromWallet)
                .Include(t => t.ToWallet)
                .OrderByDescending(t => t.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Transaction>> SearchAsync(TransactionSearchRequest dto)
        {
            var query = _context.Transactions.AsQueryable();

            if (dto.WalletId.HasValue)
                query = query.Where(t => t.FromWalletId == dto.WalletId.Value || t.ToWalletId == dto.WalletId.Value);

            if (dto.FromDate.HasValue)
                query = query.Where(t => t.CreatedAt >= dto.FromDate.Value);

            if (dto.ToDate.HasValue)
                query = query.Where(t => t.CreatedAt <= dto.ToDate.Value);

            if (dto.MinAmount.HasValue)
                query = query.Where(t => t.Amount >= dto.MinAmount.Value);

            if (dto.MaxAmount.HasValue)
                query = query.Where(t => t.Amount <= dto.MaxAmount.Value);

            return await query
                .Include(t => t.FromWallet)
                .Include(t => t.ToWallet)
                .OrderByDescending(t => t.CreatedAt)
                .Skip((dto.PageNumber - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync();
        }

        public async Task<int> CountSearchAsync(TransactionSearchRequest dto)
        {
            var query = _context.Transactions.AsQueryable();

            if (dto.WalletId.HasValue)
                query = query.Where(t => t.FromWalletId == dto.WalletId.Value || t.ToWalletId == dto.WalletId.Value);

            if (dto.FromDate.HasValue)
                query = query.Where(t => t.CreatedAt >= dto.FromDate.Value);

            if (dto.ToDate.HasValue)
                query = query.Where(t => t.CreatedAt <= dto.ToDate.Value);

            if (dto.MinAmount.HasValue)
                query = query.Where(t => t.Amount >= dto.MinAmount.Value);

            if (dto.MaxAmount.HasValue)
                query = query.Where(t => t.Amount <= dto.MaxAmount.Value);

            return await query.CountAsync();
        }

        public async Task<List<Transaction>> SearchAsync(DateTime? fromDate, DateTime? toDate, string? walletName)
        {
            var query = _context.Transactions
                .Include(t => t.FromWallet)
                .Include(t => t.ToWallet)
                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(t => t.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(t => t.CreatedAt <= toDate.Value);

            if (!string.IsNullOrEmpty(walletName))
                query = query.Where(t =>
                    t.FromWallet.Name.Contains(walletName) ||
                    t.ToWallet.Name.Contains(walletName)
                );

            return await query
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> CountSearchAsync(DateTime? fromDate, DateTime? toDate, string? walletName)
        {
            var query = _context.Transactions.AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(t => t.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(t => t.CreatedAt <= toDate.Value);

            if (!string.IsNullOrEmpty(walletName))
                query = query.Where(t =>
                    t.FromWallet.Name.Contains(walletName) ||
                    t.ToWallet.Name.Contains(walletName)
                );

            return await query.CountAsync();
        }

    }
}
