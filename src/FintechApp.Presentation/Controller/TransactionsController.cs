using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FintechApp.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _service;

    public TransactionsController(ITransactionService service)
    {
        _service = service;
    }

    // GET api/Transactions/me/wallet/{walletId}?pageNumber=1&pageSize=20
    // User
    [HttpGet("me/wallet/{walletId}")]
    public async Task<IActionResult> GetMyTransactionsByWalletPaged(int walletId, int pageNumber = 1, int pageSize = 20)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized(new { message = "Invalid token or missing userId" });

        var userId = int.Parse(userIdClaim);
        var result = await _service.GetMyWalletTransaction(userId, walletId, pageNumber, pageSize);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // GET api/Transactions/wallet/{walletId}?pageNumber=1&pageSize=20
    // Admin
    [HttpGet("wallet/{walletId}")]
    public async Task<IActionResult> GetByWalletPaged(int walletId, int pageNumber = 1, int pageSize = 20)
    {
        var result = await _service.GetByWalletPagedAsync(walletId, pageNumber, pageSize);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // GET api/Transactions/{id}
    // Admin 
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTransactionById(int id)
    {
        var result = await _service.GetTransactionByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }
}
