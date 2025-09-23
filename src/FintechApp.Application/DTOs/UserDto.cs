using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FintechApp.Application.DTOs
{
    public record UserCreateRequest(string UserName, string Email, string Password);
    public record UserUpdateRequest(int UserId, string UserName, string Email, bool IsActive);
    public record UserResponse(int UserId, string UserName, string Email, bool IsActive);
    public record UserLoginDto(string Email, string Password);
    public record LoginResponseDto
    (
         int UserId,
         string UserName,
         string Email,
         string Token,
         string RoleName
    );
    public record RoleDto(
        int RoleId,
        string RoleName
    );

    public record UserDto(
        int UserId,
        string UserName,
        string Email,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        List<WalletDto> Wallets,
        List<RoleDto> Roles
    );

}
