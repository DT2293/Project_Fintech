using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FintechApp.Domain.Entities;
using FintechApp.Domain.Interfaces;

namespace FintechApp.Infrastructure.Repositories
{
    public class TransactionEntryRepository : Repository<TransactionEntry>, ITransactionEntryRepository
    {
        public TransactionEntryRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<TransactionEntry>> GetByWalletAsync(int walletId) =>
            await FindAsync(e => e.WalletId == walletId);

        public async Task<List<TransactionEntry>> GetByTransactionAsync(int transactionId) =>
            await FindAsync(e => e.TransactionId == transactionId);
    }
}
