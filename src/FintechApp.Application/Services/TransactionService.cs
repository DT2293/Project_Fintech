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

        public async Task<ApiResponse<TransactionResponse>> CreateAsync(TransactionCreateRequest dto)
        {
            var fromWallet = await _unitOfWork.UserWallets.GetWalletWithCurrencyAsync(dto.FromWalletId);
            var toWallet = await _unitOfWork.UserWallets.GetWalletWithCurrencyAsync(dto.ToWalletId);

            if (fromWallet == null || toWallet == null)
                return ApiResponse<TransactionResponse>.Fail("Wallet not found");

            if (fromWallet.CurrencyId != toWallet.CurrencyId)
                return ApiResponse<TransactionResponse>.Fail("Wallets must use the same currency");

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
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Transactions.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                await transaction.CommitAsync();

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
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<ApiResponse<Transaction>> GetByIdAsync(int transactionId)
        {
            var t = await _unitOfWork.Transactions.GetByIdAsync(transactionId);
            if (t == null) return ApiResponse<Transaction>.Fail("Transaction not found");

            return ApiResponse<Transaction>.SuccessResponse(t);
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