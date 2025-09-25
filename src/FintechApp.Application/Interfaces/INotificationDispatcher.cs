using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FintechApp.Application.Interfaces
{
    public interface INotificationDispatcher
    {
        Task SendToUserAsync(int userId, object payload);

    }
}
