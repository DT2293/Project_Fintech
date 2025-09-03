using FintechApp.Application.DTOs;
using FintechApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FintechApp.Presentation.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateRequest dto)
        {
            var result = await _userService.RegisterAsync(dto);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _userService.GetAllPagedAsync(pageNumber, pageSize);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _userService.GetByIdAsync(id);
            if (result.Success) return Ok(result);
            return NotFound(result);
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string name, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _userService.SearchByNamePagedAsync(name, pageNumber, pageSize);
            if (result.Success) return Ok(result);
            return NotFound(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserUpdateRequest dto)
        {
            var result = await _userService.UpdateAsync(id, dto);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteAsync(id);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("{userId}/roles/{roleId}")]
        public async Task<IActionResult> AssignRole(int userId, int roleId)
        {
            var result = await _userService.AssignRoleAsync(userId, roleId);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("{userId}/roles")]
        public async Task<IActionResult> GetRoles(int userId)
        {
            var result = await _userService.GetRolesAsync(userId);
            if (result.Success) return Ok(result);
            return NotFound(result);
        }
    }
}
