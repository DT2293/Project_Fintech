using FintechApp.Application.DTOs;
using FintechApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        /// <summary>
        /// Registers a new user account with the provided information.
        /// </summary>
        /// <param name="dto">The user creation request containing username, email, password, and other details.</param>
        /// <returns>Returns the registration result. If successful, returns 200 OK; otherwise, 400 Bad Request.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateRequest dto)
        {
            var result = await _userService.RegisterAsync(dto);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Authenticates a user with credentials and issues an access token if valid.
        /// </summary>
        /// <param name="dto">The login request containing username/email and password.</param>
        /// <returns>Returns 200 OK with token if successful; otherwise, 401 Unauthorized.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var result = await _userService.LoginAsync(dto);
            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }
        /// <summary>
        /// Retrieves a paginated list of all users in the system.
        /// </summary>
        /// <param name="pageNumber">The page number (default is 1).</param>
        /// <param name="pageSize">The number of users per page (default is 10).</param>
        /// <returns>Returns 200 OK with user list if successful; otherwise, 400 Bad Request.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllUser([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _userService.GetAllUserServiceAsync(pageNumber, pageSize, User);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
        /// <summary>
        /// Retrieves a user's details by their unique identifier.
        /// </summary>
        /// <param name="id">The unique ID of the user.</param>
        /// <returns>Returns 200 OK with user details if found; otherwise, 404 Not Found.</returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdUser(int id)
        {
            var result = await _userService.GetUserByIdServiceAsync(id);
            if (result.Success) return Ok(result);
            return NotFound(result);
        }
        /// <summary>
        /// Searches for users by their name with pagination support.
        /// </summary>
        /// <param name="name">The name or partial name of the user.</param>
        /// <param name="pageNumber">The page number (default is 1).</param>
        /// <param name="pageSize">The number of users per page (default is 10).</param>
        /// <returns>Returns 200 OK with matched users if found; otherwise, 404 Not Found.</returns>
        [Authorize]
        [HttpGet("search")]
        public async Task<IActionResult> SearchByName([FromQuery] string name, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _userService.SearchByNamePagedAsync(name, pageNumber, pageSize);
            if (result.Success) return Ok(result);
            return NotFound(result);
        }
        /// <summary>
        /// Updates an existing user's information by their ID.
        /// </summary>
        /// <param name="id">The unique ID of the user to update.</param>
        /// <param name="dto">The update request containing new user information.</param>
        /// <returns>Returns 200 OK if update is successful; otherwise, 400 Bad Request.</returns>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserInfo(int id, UserUpdateRequest dto)
        {
            var result = await _userService.UpdateAsync(id, dto);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
        /// <summary>
        /// Deletes a user from the system by their ID.
        /// </summary>
        /// <param name="id">The unique ID of the user to delete.</param>
        /// <returns>Returns 200 OK if deletion is successful; otherwise, 400 Bad Request.</returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteAsync(id);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Assigns a specific role to a user.
        /// </summary>
        /// <param name="userId">The unique ID of the user.</param>
        /// <param name="roleId">The unique ID of the role to assign.</param>
        /// <returns>Returns 200 OK if assignment is successful; otherwise, 400 Bad Request.</returns>
        [Authorize]
        [HttpPost("{userId}/roles/{roleId}")]
        public async Task<IActionResult> AssignRole (int userId, int roleId)
        {
            var result = await _userService.AssignRoleAsync(userId, roleId);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
        /// <summary>
        /// Retrieves all roles assigned to a specific user.
        /// </summary>
        /// <param name="userId">The unique ID of the user.</param>
        /// <returns>Returns 200 OK with the list of roles if successful; otherwise, 404 Not Found.</returns>
        //[HttpGet("{userId}/roles")]
        //public async Task<IActionResult> GetAllUserRoles(int userId)
        //{
        //    var result = await _userService.GetRolesAsync(userId);
        //    if (result.Success) return Ok(result);
        //    return NotFound(result);
        //}
        /// <summary>
        /// A simple endpoint to test if the controller is reachable.
        /// </summary>
        /// <returns></returns>
        [HttpGet("hi")]
        public IActionResult GetStatistics()
        {
            return Ok("Hello");
        }

    }
}
