using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FintechApp.Domain.Entities
{
    public class ApiPermission
    {
        [Key]
        public int Id { get; set; }
        public string Controller { get; set; } = default!;
        public string Action { get; set; } = default!;
        public int PermissionId { get; set; }
        public Permission Permission { get; set; } = default!;
    }
}
