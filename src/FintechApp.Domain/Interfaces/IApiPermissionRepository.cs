using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FintechApp.Domain.Entities;

namespace FintechApp.Domain.Interfaces
{
    public interface IApiPermissionRepository
    {
        Task<Permission?> GetPermissionAsync(string controller, string action);
    }
}
