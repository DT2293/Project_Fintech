using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FintechApp.Application.DTOs
{
    public record WalletDto(int WalletId, string name, decimal Balance, string CurrencyName);
     
}
