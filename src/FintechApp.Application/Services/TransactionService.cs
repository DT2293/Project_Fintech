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

        public async Task<ApiResponse<Transaction>> CreateAsync(TransactionCreateRequest dto)
        {
            // Lấy 2 ví
            var fromWallet = await _unitOfWork.UserWallets.GetByIdAsync(dto.FromWalletId);
            var toWallet = await _unitOfWork.UserWallets.GetByIdAsync(dto.ToWalletId);

            if (fromWallet == null || toWallet == null)
                return ApiResponse<Transaction>.Fail("Wallet not found");

            if (fromWallet.Balance < dto.Amount)
                return ApiResponse<Transaction>.Fail("Insufficient balance");

            // Transaction EF Core
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                fromWallet.Balance -= dto.Amount;
                toWallet.Balance += dto.Amount;

                var t = new Transaction
                {
                    FromWalletId = dto.FromWalletId,
                    ToWalletId = dto.ToWalletId,
                    Amount = dto.Amount,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Transactions.AddAsync(t);
                await _unitOfWork.SaveChangesAsync();

                await transaction.CommitAsync();

                return ApiResponse<Transaction>.SuccessResponse(t, "Transaction created successfully");
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

        public async Task<PagedResponse<Transaction>> GetByWalletPagedAsync(int walletId, int pageNumber, int pageSize)
        {
            var total = await _unitOfWork.Transactions.CountByWalletIdAsync(walletId);
            var list = await _unitOfWork.Transactions.GetPagedByWalletAsync(walletId, pageNumber, pageSize);

            return new PagedResponse<Transaction>
            {
                Success = true,
                Data = list,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = total
            };
        }

        public async Task<PagedResponse<Transaction>> SearchAsync(TransactionSearchRequest dto)
        {
            var total = await _unitOfWork.Transactions.CountSearchAsync(dto);
            var list = await _unitOfWork.Transactions.SearchAsync(dto);

            return new PagedResponse<Transaction>
            {
                Success = true,
                Data = list,
                PageNumber = dto.PageNumber,
                PageSize = dto.PageSize,
                TotalRecords = total
            };
        }
    }
