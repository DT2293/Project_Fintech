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
}
