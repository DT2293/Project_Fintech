using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FintechApp.Application.Common;
using FintechApp.Application.DTOs;
using FintechApp.Domain.Entities;

namespace FintechApp.Application.Interfaces
{
    public interface INotificationService
    {
        Task<ApiResponse<NotificationResponse>> CreateNotificationAsync(int userId, string title, string message);


        Task<PagedResponse<NotificationResponse>> GetNotificationsAsync(int userId, int pageNumber, int pageSize);

        Task<ApiResponse<List<NotificationResponse>>> GetUnreadNotificationsAsync(int userId);

        Task<ApiResponse<string>> MarkAsReadAsync(int id);


        Task<ApiResponse<string>> MarkAllAsReadAsync(int userId);
    }
}
