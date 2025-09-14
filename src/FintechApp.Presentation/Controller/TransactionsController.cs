using FintechApp.Application.DTOs;
using FintechApp.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FintechApp.Presentation.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _service;

        public TransactionsController(ITransactionService service)
        {
            _service = service;
        }

        // api service for transfer to demo transaction function 
        [HttpPost]
        public async Task<IActionResult> CreateTransaction(TransactionCreateRequest dto)
        {
            var result = await _service.CreateTransactionAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            var result = await _service.GetTransactionByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("wallet/{walletId}")]
        public async Task<IActionResult> GetByWalletPaged(int walletId, int pageNumber = 1, int pageSize = 20)
        {
            var result = await _service.GetByWalletPagedAsync(walletId, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] TransactionSearchRequest dto)
        {
            var result = await _service.SearchAsync(dto);
            return Ok(result);
        }
    }
}


