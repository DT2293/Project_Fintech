using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FintechApp.Application.Common;
using FintechApp.Application.DTOs;
using FintechApp.Application.Interfaces;
using FintechApp.Domain.Entities;
using FintechApp.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
namespace FintechApp.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;
        private readonly INotificationDispatcher _dispatcher;

        public NotificationService(
            INotificationRepository repo,
            INotificationDispatcher dispatcher)
        {
            _repo = repo;
            _dispatcher = dispatcher;
        }

        public async Task<ApiResponse<NotificationResponse>> CreateNotificationAsync(int userId, string title, string message)
        {
            var notif = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message
            };

            await _repo.AddAsync(notif);

            await _dispatcher.SendToUserAsync(userId, new
            {
                notif.UserId,
                notif.Title,
                notif.Message,
                notif.CreatedAt
            });

            var dto = new NotificationResponse(
                notif.id,
                notif.UserId,
                notif.Title,
                notif.Message,
                notif.IsRead,
                notif.CreatedAt
            );

            return ApiResponse<NotificationResponse>.SuccessResponse(dto, "Notification created successfully");
        }

        public async Task<PagedResponse<NotificationResponse>> GetNotificationsAsync(int userId, int pageNumber, int pageSize)
        {
            var query = await _repo.GetByUserIdAsync(userId);

            var totalRecords = query.Count;
            var data = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(n => new NotificationResponse(
                    n.id,
                    n.UserId,
                    n.Title,
                    n.Message,
                    n.IsRead,
                    n.CreatedAt
                ))
                .ToList();

            return new PagedResponse<NotificationResponse>
            {
                Success = true,
                Message = "Notifications fetched successfully",
                Data = data,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<ApiResponse<List<NotificationResponse>>> GetUnreadNotificationsAsync(int userId)
        {
            var query = await _repo.GetUnreadByUserIdAsync(userId);

            var data = query
                .Select(n => new NotificationResponse(
                    n.id,
                    n.UserId,
                    n.Title,
                    n.Message,
                    n.IsRead,
                    n.CreatedAt
                ))
                .ToList();

            return ApiResponse<List<NotificationResponse>>.SuccessResponse(data, "Unread notifications fetched successfully");
        }

        public async Task<ApiResponse<string>> MarkAsReadAsync(int id)
        {
            await _repo.MarkAsReadAsync(id);
            return ApiResponse<string>.SuccessResponse("OK", "Notification marked as read");
        }

        public async Task<ApiResponse<string>> MarkAllAsReadAsync(int userId)
        {
            await _repo.MarkAllAsReadAsync(userId);
            return ApiResponse<string>.SuccessResponse("OK", "All notifications marked as read");
        }


    }
}

