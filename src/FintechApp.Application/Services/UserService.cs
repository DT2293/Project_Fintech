using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FintechApp.Application.Common;
using FintechApp.Application.DTOs;
using FintechApp.Application.Interfaces;
using FintechApp.Domain.Entities;
using FintechApp.Domain.Interfaces;

namespace FintechApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<User>> RegisterAsync(UserCreateRequest dto)
        {
            var existing = await _unitOfWork.Users.GetByEmailAsync(dto.Email);
            if (existing != null)
                return ApiResponse<User>.Fail("Email already exists");

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<User>.SuccessResponse(user, "User registered successfully");
        }

        public async Task<ApiResponse<User>> UpdateAsync(int userId, UserUpdateRequest dto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                return ApiResponse<User>.Fail("User not found");

            user.UserName = dto.UserName ?? user.UserName;

            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<User>.SuccessResponse(user, "User updated successfully");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                return ApiResponse<bool>.Fail("User not found");

            _unitOfWork.Users.Remove(user);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "User deleted successfully");
        }
        public async Task<PagedResponse<User>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            var totalRecords = await _unitOfWork.Users.CountAsync();
            var users = await _unitOfWork.Users.GetPagedAsync(pageNumber, pageSize);

            return new PagedResponse<User>
            {
                Success = true,
                Data = users,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Message = "Users retrieved successfully"
            };
        }

        public async Task<PagedResponse<User>> SearchByNamePagedAsync(string name, int pageNumber, int pageSize)
        {
            var totalRecords = await _unitOfWork.Users.CountByNameAsync(name);
            var users = await _unitOfWork.Users.SearchByNamePagedAsync(name, pageNumber, pageSize);

            return new PagedResponse<User>
            {
                Success = true,
                Data = users,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Message = $"Users matching '{name}' retrieved successfully"
            };
        }

        public async Task<ApiResponse<User>> GetByIdAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                return ApiResponse<User>.Fail("User not found");

            return ApiResponse<User>.SuccessResponse(user);
        }

        public async Task<ApiResponse<List<User>>> GetAllAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return ApiResponse<List<User>>.SuccessResponse(users);
        }

        public async Task<ApiResponse<List<User>>> SearchByNameAsync(string name)
        {
            var users = await _unitOfWork.Users.SearchByNameAsync(name);
            return ApiResponse<List<User>>.SuccessResponse(users);
        }

        public async Task<ApiResponse<bool>> AssignRoleAsync(int userId, int roleId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                return ApiResponse<bool>.Fail("User not found");

            var role = await _unitOfWork.Roles.GetByIdAsync(roleId);
            if (role == null)
                return ApiResponse<bool>.Fail("Role not found");

            if (user.UserRoles.Any(ur => ur.RoleId == roleId))
                return ApiResponse<bool>.Fail("Role already assigned");

            user.UserRoles.Add(new UserRole { UserId = userId, RoleId = roleId });
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Role assigned successfully");
        }

        public async Task<ApiResponse<List<Role>>> GetRolesAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdWithRolesAsync(userId);
            if (user == null)
                return ApiResponse<List<Role>>.Fail("User not found");

            var roles = user.UserRoles.Select(ur => ur.Role).ToList();
            return ApiResponse<List<Role>>.SuccessResponse(roles);
        }

        // Helper hash
        private string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
        private bool VerifyPassword(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
