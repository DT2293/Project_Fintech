using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FintechApp.Domain.Interfaces
{
    public interface IPermissionService
    {
        Task<bool> HasPermissionAsync(int userId, string controller, string action);
    }

}
