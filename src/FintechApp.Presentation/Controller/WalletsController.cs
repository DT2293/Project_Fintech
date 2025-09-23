using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FintechApp.Application.Interfaces;

namespace FintechApp.Presentation.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletsController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyWallets(int pageNumber = 1, int pageSize = 20)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { message = "Invalid token or missing userId" });

            var userId = int.Parse(userIdClaim);
            var result = await _walletService.GetMyWalletsPagedAsync(userId, pageNumber, pageSize);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // GET api/Wallets?pageNumber=1&pageSize=20  (admin/view-all)
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllWallets(int pageNumber = 1, int pageSize = 20)
        {
            var result = await _walletService.GetAllWalletsPagedAsync(pageNumber, pageSize);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
