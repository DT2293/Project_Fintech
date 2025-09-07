using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FintechApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FintechApp.Infrastructure.Repositories
{
    public class PermissionRepo : IPermissionService
    {
        private readonly ApplicationDbContext _context;

        public PermissionRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HasPermissionAsync(int userId, string controller, string action)
        {
            var apiPerm = await _context.ApiPermissions
                .Include(a => a.Permission)
                .FirstOrDefaultAsync(a => a.Controller == controller && a.Action == action);

            if (apiPerm == null) return false;

            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .ThenInclude(r => r.RolePermissions)
                .AnyAsync(u => u.UserId == userId &&
                               u.UserRoles.Any(r =>
                                   r.Role.RolePermissions.Any(rp => rp.PermissionId == apiPerm.PermissionId)));
        }
    }
}
