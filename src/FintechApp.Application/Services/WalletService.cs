using FintechApp.Application.Common;
using FintechApp.Application.DTOs;
using FintechApp.Application.Interfaces;
using FintechApp.Domain.Interfaces;
using System.Linq;
using System.Threading.Tasks;

public class WalletService : IWalletService
{
    private readonly IUnitOfWork _unitOfWork;

    public WalletService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // User lấy ví của chính mình (phân trang ở service)
    public async Task<ApiResponse<PagedResponse<WalletDto>>> GetMyWalletsPagedAsync(int userId, int pageNumber, int pageSize)
    {
        var totalCount = await _unitOfWork.UserWallets.CountByUserAsync(userId);
        var wallets = await _unitOfWork.UserWallets.GetByUserAsync(userId);

        var dtoList = wallets
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(w => new WalletDto(
                w.WalletId,
                w.Name,
                w.Balance,
                w.Currency != null ? w.Currency.Name : string.Empty
            ))

            .ToList();

        var pagedResponse = new PagedResponse<WalletDto>
        {
            Success = true,
            Data = dtoList,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalCount
        };

        return ApiResponse<PagedResponse<WalletDto>>.SuccessResponse(pagedResponse);
    }

    // Admin lấy tất cả ví (phân trang ở service)
    public async Task<ApiResponse<PagedResponse<WalletDto>>> GetAllWalletsPagedAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _unitOfWork.UserWallets.CountAllAsync();
        var wallets = await _unitOfWork.UserWallets.GetAllAsync();
        var dtoList = wallets
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(w => new WalletDto(
                w.WalletId,
                w.Name,
                w.Balance,
                w.Currency != null ? w.Currency.Name : string.Empty
            ))
            .ToList();


        var pagedResponse = new PagedResponse<WalletDto>
        {
            Success = true,
            Data = dtoList,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalCount
        };

        return ApiResponse<PagedResponse<WalletDto>>.SuccessResponse(pagedResponse);
    }
}
