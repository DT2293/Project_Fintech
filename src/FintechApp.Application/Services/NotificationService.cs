using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<Notification> CreateNotificationAsync(int userId, string title, string message)
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

            return notif;
        }

      
        public Task<List<Notification>> GetNotificationsAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task MarkAsReadAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}

