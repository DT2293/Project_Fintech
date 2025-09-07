using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FintechApp.Domain.Entities;

namespace FintechApp.Domain.Interfaces
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<List<Transaction>> GetByWalletIdAsync(int walletId);
        Task<int> CountByWalletIdAsync(int walletId);
        Task<List<Transaction>> GetPagedByWalletAsync(int walletId, int pageNumber, int pageSize);
        Task<Transaction?> GetTransactionById(int id);

        Task<List<Transaction>> SearchAsync(DateTime? fromDate, DateTime? toDate, int? walletId, int pageNumber, int pageSize);
        Task<int> CountSearchAsync(DateTime? fromDate, DateTime? toDate, int? walletId);

    }
}
