using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FintechApp.Domain.Entities
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int FromWalletId { get; set; }
        public UserWallet FromWallet { get; set; } = null!;

        public int ToWalletId { get; set; }
        public UserWallet ToWallet { get; set; } = null!;
    }
}
