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
        // Lấy danh sách ví theo UserId
        Task<List<UserWallet>> GetByUserAsync(int userId);

        // Lấy tất cả ví (admin)
        Task<List<UserWallet>> GetAllAsync();

        // Lấy ví theo WalletId
        Task<UserWallet?> GetWalletByIdAsync(int walletId);

        // Lấy ví theo WalletId kèm thông tin Currency
        Task<UserWallet?> GetByIdWithCurrencyAsync(int walletId);

        // Đếm số ví của 1 user
        Task<int> CountByUserAsync(int userId);

        // Đếm tất cả ví
        Task<int> CountAllAsync();

        Task<UserWallet?> GetWalletWithCurrencyAsync(int walletId);
    }
}


