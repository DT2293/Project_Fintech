using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FintechApp.Domain.Entities;

namespace FintechApp.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetWithRolesAsync(int userId);
        Task<User?> GetByIdWithRolesAsync(int id);
        Task<List<User>> SearchByNameAsync(string name);
        Task<int> CountAsync();
        Task<int> CountByNameAsync(string name);
        Task<List<User>> GetPagedAsync(int pageNumber, int pageSize);
        Task<List<User>> SearchByNamePagedAsync(string name, int pageNumber, int pageSize);
    }
}
