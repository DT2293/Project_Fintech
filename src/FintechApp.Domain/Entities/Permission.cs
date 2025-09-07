using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FintechApp.Domain.Entities
{
    public class Permission
    {
        public int PermissionId { get; set; }
        public string Name { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty;

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
        public ICollection<ApiPermission> ApiPermissions { get; set; } = new List<ApiPermission>();
    }
}
