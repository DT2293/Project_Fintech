using FintechApp.Application.Interfaces;
using FintechApp.Application.Services;
using FintechApp.Domain.Interfaces;
using FintechApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FintechApp.Presentation.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserWalletRepository, UserWalletRepository>();
            
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            // UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Application Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITransactionService, TransactionService>();

            return services;
        }
    }
}
