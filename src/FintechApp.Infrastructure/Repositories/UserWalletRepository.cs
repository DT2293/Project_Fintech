using FintechApp.Domain.Entities;
using FintechApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FintechApp.Infrastructure.Repositories
{
    public class UserWalletRepository : Repository<UserWallet>, IUserWalletRepository
    {
        public UserWalletRepository(ApplicationDbContext context) : base(context) { }

        public async Task<UserWallet?> GetWalletWithCurrencyAsync(int walletId)
        {
            return await _context.UserWallets
                .Include(w => w.Currency) // load luôn currency
                .FirstOrDefaultAsync(w => w.WalletId == walletId);
        }

        // Lấy tất cả ví theo User
        public async Task<List<UserWallet>> GetByUserAsync(int userId)
        {
            return await _dbSet
                .Where(w => w.UserId == userId)
                .OrderBy(w => w.WalletId)
                .ToListAsync();
        }

        // Lấy tất cả ví (cho admin)
        public async Task<List<UserWallet>> GetAllAsync()
        {
            return await _dbSet
                .OrderBy(w => w.WalletId)
                .ToListAsync();
        }

        // Lấy ví theo WalletId
        public Task<UserWallet?> GetWalletByIdAsync(int walletId)
        {
            return _dbSet.FirstOrDefaultAsync(w => w.WalletId == walletId);
        }

        // Lấy ví theo WalletId kèm thông tin Currency
        public async Task<UserWallet?> GetByIdWithCurrencyAsync(int walletId)
        {
            return await _dbSet
                .Include(w => w.Currency)
                .FirstOrDefaultAsync(w => w.WalletId == walletId);
        }

        // Đếm ví của user
        public async Task<int> CountByUserAsync(int userId)
        {
            return await _dbSet.CountAsync(w => w.UserId == userId);
        }

        // Đếm tất cả ví
        public async Task<int> CountAllAsync()
        {
            return await _dbSet.CountAsync();
        }
    }
}
