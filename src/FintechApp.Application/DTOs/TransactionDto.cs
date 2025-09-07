using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FintechApp.Application.DTOs
{
    public record TransactionCreateRequest(int FromWalletId, int ToWalletId, decimal Amount);
    public record TransactionSearchRequest(
        int? WalletId = null, 
        DateTime? FromDate = null, 
        DateTime? ToDate = null, 
        decimal? MinAmount = null,
        decimal? MaxAmount = null,
        int PageNumber = 1,
        int PageSize = 20 
        );
    public record TransactionResponse(
         int TransactionId,
         decimal Amount,
         DateTime CreatedAt,
         string FromWalletName,
         string ToWalletName,
         string CurrencyName 
     );

    public record TransactionInfoDto(
     int TransactionId,
     decimal Amount,
     DateTime CreatedAt,
     string FromWalletName,
     string ToWalletName,
     string CurrencyName,
     string FromUserName,
     string ToUserName
 );


    public record WalletInfo(int WalletId, string Name, string CurrencyCode);

}
