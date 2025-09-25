using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FintechApp.Domain.Entities;

namespace FintechApp.Application.Interfaces
{
    public interface INotificationService
    {
        Task<Notification> CreateNotificationAsync(int userId, string title, string message);
        Task<List<Notification>> GetNotificationsAsync(int userId);
        Task MarkAsReadAsync(int id);
    }
}
