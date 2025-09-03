using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FintechApp.Domain.Entities
{
    public class UserWallet
    {
        public int WalletId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; } = 0;
        public int UserId { get; set; }
        public int CurrencyId { get; set; }

        public User User { get; set; } = null!;
        public Currency Currency { get; set; } = null!;

        public ICollection<Transaction> FromTransactions { get; set; } = new List<Transaction>();
        public ICollection<Transaction> ToTransactions { get; set; } = new List<Transaction>();

    }
}
