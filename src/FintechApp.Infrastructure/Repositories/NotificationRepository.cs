using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FintechApp.Domain.Entities;
using FintechApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FintechApp.Infrastructure.Repositories
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Notification>> GetByUserIdAsync(int userId)
        {
            return await _context.notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Notification>> GetUnreadByUserIdAsync(int userId)
        {
            return await _context.notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notif = await _context.notifications
                .FirstOrDefaultAsync(n => n.id == notificationId);

            if (notif != null && !notif.IsRead)
            {
                notif.IsRead = true;
                _context.notifications.Update(notif);
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            var notifs = await _context.notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            if (notifs.Any())
            {
                foreach (var n in notifs)
                {
                    n.IsRead = true;
                }

                _context.notifications.UpdateRange(notifs);
                await _context.SaveChangesAsync();
            }
        }
    }
}
