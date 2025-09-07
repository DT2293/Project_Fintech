using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FintechApp.Application.Common;
using FintechApp.Application.DTOs;
using FintechApp.Domain.Entities;

namespace FintechApp.Application.Interfaces
{
    public interface IUserService
    {
        // Register & Login
        Task<ApiResponse<User>> RegisterAsync(UserCreateRequest dto);
        Task<ApiResponse<LoginResponseDto>> LoginAsync(UserLoginDto dto);

        // CRUD
        Task<ApiResponse<UserDto>> GetUserByIdServiceAsync(int userId);
        Task<ApiResponse<User>> UpdateAsync(int userId, UserUpdateRequest dto);
        Task<ApiResponse<bool>> DeleteAsync(int userId);
        Task<PagedResponse<UserDto>> GetAllUserServiceAsync(int pageNumber, int pageSize);
        Task<PagedResponse<UserDto>> SearchByNamePagedAsync(string name, int pageNumber, int pageSize);
        // Role management
        Task<ApiResponse<bool>> AssignRoleAsync(int userId, int roleId);
        Task<ApiResponse<List<Role>>> GetRolesAsync(int userId);
    }
}
