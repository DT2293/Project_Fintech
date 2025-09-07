using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FintechApp.Application.Common;
using FintechApp.Application.DTOs;
using FintechApp.Application.Interfaces;
using FintechApp.Domain.Entities;
using FintechApp.Domain.Interfaces;

namespace FintechApp.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<TransactionResponse>> CreateTransactionAsync(TransactionCreateRequest dto)
        {
            // 1. Lấy ví từ DB
            var fromWallet = await _unitOfWork.UserWallets.GetWalletWithCurrencyAsync(dto.FromWalletId);
            var toWallet = await _unitOfWork.UserWallets.GetWalletWithCurrencyAsync(dto.ToWalletId);

            if (fromWallet == null || toWallet == null)
                return ApiResponse<TransactionResponse>.Fail("Wallet not found");

            if (fromWallet.CurrencyId != toWallet.CurrencyId)
                return ApiResponse<TransactionResponse>.Fail("Wallets must use the same currency");

            if (dto.FromWalletId == dto.ToWalletId)
                return ApiResponse<TransactionResponse>.Fail("FromWallet and ToWallet cannot be the same");

            if (fromWallet.Balance < dto.Amount)
                return ApiResponse<TransactionResponse>.Fail("Insufficient balance");

            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                fromWallet.Balance -= dto.Amount;
                toWallet.Balance += dto.Amount;
                var entity = new Transaction
                {
                    FromWalletId = dto.FromWalletId,
                    ToWalletId = dto.ToWalletId,
                    Amount = dto.Amount,
                    CreatedAt = DateTime.UtcNow,
                    Type = TransactionType.Transfer,
                    Status = TransactionStatus.Success
                };
                await _unitOfWork.Transactions.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync(); 

                // from entry
                var fromEntry = new TransactionEntry
                {
                    TransactionId = entity.TransactionId,
                    WalletId = fromWallet.WalletId,
                    Amount = dto.Amount,
                    EntryType = EntryType.Debit
                };
                // to entry
                var toEntry = new TransactionEntry
                {
                    TransactionId = entity.TransactionId,
                    WalletId = toWallet.WalletId,
                    Amount = dto.Amount,
                    EntryType = EntryType.Credit
                };

                await _unitOfWork.TransactionEntries.AddAsync(fromEntry);
                await _unitOfWork.TransactionEntries.AddAsync(toEntry);

                // Commit 
                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();

                // response
                var response = new TransactionResponse(
                    entity.TransactionId,
                    entity.Amount,
                    entity.CreatedAt,
                    fromWallet.Name,
                    toWallet.Name,
                    fromWallet.Currency.Name
                );

                return ApiResponse<TransactionResponse>.SuccessResponse(response, "Transaction created successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ApiResponse<TransactionResponse>.Fail("Transaction failed: " + ex.Message);
            }
        }



        public async Task<ApiResponse<TransactionInfoDto>> GetTransactionByIdAsync(int transactionId)
        {
            var t = await _unitOfWork.Transactions
                .GetTransactionById(transactionId); 

            if (t == null)
                return ApiResponse<TransactionInfoDto>.Fail("Transaction not found");

            var dto = new TransactionInfoDto(
                t.TransactionId,
                t.Amount,
                t.CreatedAt,
                t.FromWallet.Name,
                t.ToWallet.Name,
                t.FromWallet.Currency.Name,
                t.FromWallet.User.UserName,
                t.ToWallet.User.UserName
            );

            return ApiResponse<TransactionInfoDto>.SuccessResponse(dto);
        }


        public async Task<PagedResponse<TransactionResponse>> GetByWalletPagedAsync(int walletId, int pageNumber, int pageSize)
        {
            var total = await _unitOfWork.Transactions.CountByWalletIdAsync(walletId);
            var list = await _unitOfWork.Transactions.GetPagedByWalletAsync(walletId, pageNumber, pageSize);

            var responseList = list.Select(t => new TransactionResponse(
                t.TransactionId,
                t.Amount,
                t.CreatedAt,
                t.FromWallet.Name,
                t.ToWallet.Name,
                t.FromWallet.Currency.Name
            )).ToList();

            return new PagedResponse<TransactionResponse>
            {
                Success = true,
                Data = responseList,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = total
            };
        }


        public async Task<PagedResponse<TransactionResponse>> SearchAsync(TransactionSearchRequest dto)
        {
            var total = await _unitOfWork.Transactions.CountSearchAsync(
                dto.FromDate,
                dto.ToDate,
                dto.WalletId
            );

            var list = await _unitOfWork.Transactions.SearchAsync(
                dto.FromDate,
                dto.ToDate,
                dto.WalletId,
                dto.PageNumber,
                dto.PageSize
            );

            var responseList = list.Select(t => new TransactionResponse(
                t.TransactionId,
                t.Amount,
                t.CreatedAt,
                t.FromWallet.Name,
                t.ToWallet.Name,
                t.FromWallet.Currency.Name
            )).ToList();

            return new PagedResponse<TransactionResponse>
            {
                Success = true,
                Data = responseList,
                PageNumber = dto.PageNumber,
                PageSize = dto.PageSize,
                TotalRecords = total
            };
        }

        
    }
}