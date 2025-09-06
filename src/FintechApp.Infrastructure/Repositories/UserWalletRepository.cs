using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FintechApp.Domain.Entities;
using FintechApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FintechApp.Infrastructure.Repositories
{
    public class UserWalletRepository : Repository<UserWallet>, IUserWalletRepository
    {
        public UserWalletRepository(ApplicationDbContext context) : base(context) { }
        public async Task<UserWallet?> GetByUserWalletIdAsync(int userId)
        {
            return await _dbSet.FirstOrDefaultAsync(uw => uw.UserId == userId);
        }

        public async Task<UserWallet?> GetWalletWithCurrencyAsync(int walletId)
        {
            return await _dbSet
            .Include(w => w.Currency)
            .FirstOrDefaultAsync(w => w.WalletId == walletId);
        }
 

    }
}
