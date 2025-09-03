using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FintechApp.Domain.Entities
{
    public class Currency
    {
        public int CurrencyId { get; set; }
        public string Code { get; set; } = string.Empty; // USD, VND, BTC
        public string Name { get; set; } = string.Empty;

        public ICollection<UserWallet> Wallets { get; set; } = new List<UserWallet>();
    }
}
