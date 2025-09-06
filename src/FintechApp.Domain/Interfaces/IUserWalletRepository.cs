using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FintechApp.Domain.Entities;

namespace FintechApp.Domain.Interfaces
{
    public interface IUserWalletRepository : IRepository<UserWallet>
    {
        Task<UserWallet?> GetByUserWalletIdAsync(int userId);
        Task<UserWallet?> GetWalletWithCurrencyAsync(int walletId);
    }
}
