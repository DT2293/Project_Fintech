using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FintechApp.Domain.Entities;
using FintechApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FintechApp.Infrastructure.Repositories
{
    public class ApiPermissionRepository : IApiPermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public ApiPermissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Permission?> GetPermissionAsync(string controller, string action)
        {
            return await _context.ApiPermissions
                .Where(a => a.Controller.ToLower() == controller.ToLower()
                         && a.Action.ToLower() == action.ToLower())
                .Select(a => a.Permission)
                .FirstOrDefaultAsync();
        }
    }

}
