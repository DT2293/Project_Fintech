using FintechApp.Application.Interfaces;
using FintechApp.Presentation.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace FintechApp.Presentation.Services
{
    public class SignalRNotificationDispatcher : INotificationDispatcher
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public SignalRNotificationDispatcher(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendToUserAsync(int userId, object payload)
        {
            await _hubContext.Clients.Group(userId).SendAsync("ReceiveNotification", payload);
        }

        
    }
}
