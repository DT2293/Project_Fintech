using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FintechApp.Domain.Entities;

namespace FintechApp.Domain.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<List<Notification>> GetByUserIdAsync(int userId);

        // Lấy danh sách thông báo chưa đọc
        Task<List<Notification>> GetUnreadByUserIdAsync(int userId);

        // Đánh dấu 1 thông báo đã đọc
        Task MarkAsReadAsync(int notificationId);

        // Đánh dấu tất cả thông báo của user đã đọc
        Task MarkAllAsReadAsync(int userId);
    }
}
