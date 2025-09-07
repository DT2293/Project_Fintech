using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FintechApp.Domain.Entities;

namespace FintechApp.Domain.Interfaces
{
    public interface ITransactionEntryRepository : IRepository<TransactionEntry>
    {
        Task<List<TransactionEntry>> GetByWalletAsync(int walletId);
        Task<List<TransactionEntry>> GetByTransactionAsync(int transactionId);
    }
}
