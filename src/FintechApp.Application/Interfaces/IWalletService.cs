using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FintechApp.Application.Common;
using FintechApp.Application.DTOs;

namespace FintechApp.Application.Interfaces
{
    public interface IWalletService
    {
        Task<ApiResponse<PagedResponse<WalletDto>>> GetMyWalletsPagedAsync(int userId, int pageNumber, int pageSize);
        Task<ApiResponse<PagedResponse<WalletDto>>> GetAllWalletsPagedAsync(int pageNumber, int pageSize);
    }
}
