using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FintechApp.Application.DTOs
{
    public record TransactionCreateRequest(int FromWalletId, int ToWalletId, decimal Amount);
    public record TransactionSearchRequest(int? WalletId = null, DateTime? FromDate = null, DateTime? ToDate = null, decimal? MinAmount = null,
        decimal? MaxAmount = null,
        int PageNumber = 1,
        int PageSize = 20 );


}
