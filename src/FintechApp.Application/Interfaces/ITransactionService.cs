using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FintechApp.Application.Common;
using FintechApp.Application.DTOs;
using FintechApp.Domain.Entities;

namespace FintechApp.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<ApiResponse<TransactionResponse>> CreateAsync(TransactionCreateRequest dto);
        Task<ApiResponse<Transaction>> GetByIdAsync(int transactionId);
        Task<PagedResponse<TransactionResponse>> GetByWalletPagedAsync(int walletId, int pageNumber, int pageSize);
        Task<PagedResponse<TransactionResponse>> SearchAsync(TransactionSearchRequest dto);
    }
}
