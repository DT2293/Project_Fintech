using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public UserService(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(UserLoginDto dto)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(dto.Email);
            if (user == null)
                return ApiResponse<LoginResponseDto>.Fail("User not found");

            if (!VerifyPassword(dto.Password, user.PasswordHash))
                return ApiResponse<LoginResponseDto>.Fail("Invalid password");

            if (!user.IsActive)
                return ApiResponse<LoginResponseDto>.Fail("User is inactive");
            var userWithRoles = await _unitOfWork.Users.GetWithRolesAsync(user.UserId);
            var roleName = userWithRoles.UserRoles.FirstOrDefault()?.Role.Name ?? string.Empty;

            string token = _jwtTokenGenerator.GenerateToken(user);

            var response = new LoginResponseDto(
                UserId: user.UserId,
                UserName: user.UserName,
                Email: user.Email,
                Token: token,
                RoleName: roleName
            );

            return ApiResponse<LoginResponseDto>.SuccessResponse(response, "Login successful");
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
        public async Task<ApiResponse<List<UserDto>>> GetAllUserServiceAsync(int pageNumber,int pageSize,ClaimsPrincipal userClaims)
        {

            if (!userClaims.IsInRole("Admin"))
            {
                return ApiResponse<List<UserDto>>.Fail("Bạn không có quyền truy cập");
            }

            var totalRecords = await _unitOfWork.Users.CountAsync();
            var users = await _unitOfWork.Users.GetAllUserAsync(pageNumber, pageSize);

            var userDtos = users.Select(u => new UserDto(
                u.UserId,
                u.UserName,
                u.Email,
                u.IsActive,
                u.CreatedAt,
                u.UpdatedAt,
                u.Wallets.Select(w => new WalletDto(
                    w.WalletId,
                    w.Name,
                    w.Balance,
                    w.Currency?.Name ?? string.Empty
                )).ToList(),
                u.UserRoles.Select(ur => new RoleDto(
                    ur.RoleId,
                    ur.Role?.Name ?? string.Empty
                )).ToList()
            )).ToList();

            return new PagedResponse<UserDto>
            {
                Success = true,
                Data = userDtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Message = "Users retrieved successfully"
            };
        }


        public async Task<PagedResponse<UserDto>> SearchByNamePagedAsync(string name, int pageNumber, int pageSize)
        {
            var totalRecords = await _unitOfWork.Users.CountByNameAsync(name);
            var users = await _unitOfWork.Users.SearchByNamePagedAsync(name, pageNumber, pageSize);

            var userDtos = users.Select(u => new UserDto(
                u.UserId,
                u.UserName,
                u.Email,
                u.IsActive,
                u.CreatedAt,
                u.UpdatedAt,
                u.Wallets.Select(w => new WalletDto(
                    w.WalletId,
                    w.Name,
                    w.Balance,
                    w.Currency?.Name ?? string.Empty
                )).ToList(),
                u.UserRoles.Select(ur => new RoleDto(
                    ur.RoleId,
                    ur.Role?.Name ?? string.Empty
                )).ToList()
            )).ToList();

            return new PagedResponse<UserDto>
            {
                Success = true,
                Data = userDtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Message = $"Users matching '{name}' retrieved successfully"
            };
        }


        public async Task<ApiResponse<UserDto>> GetUserByIdServiceAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetUserByIdRepoAsync(userId);

            if (user == null)
                return ApiResponse<UserDto>.Fail("User not found");

            var userDto = new UserDto(
                user.UserId,
                user.UserName,
                user.Email,
                user.IsActive,
                user.CreatedAt,
                user.UpdatedAt,
                user.Wallets.Select(w => new WalletDto(
                    w.WalletId,
                    w.Name,
                    w.Balance,
                    w.Currency?.Name ?? string.Empty
                )).ToList(),
                user.UserRoles.Select(ur => new RoleDto(
                    ur.RoleId,
                    ur.Role?.Name ?? string.Empty
                )).ToList()
            );

            return ApiResponse<UserDto>.SuccessResponse(userDto, "User retrieved successfully");
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
