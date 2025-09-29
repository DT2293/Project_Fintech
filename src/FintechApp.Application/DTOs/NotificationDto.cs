using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FintechApp.Application.DTOs
{
    public record NotificationResponse(
       int Id,
       int UserId,
       string Title,
       string Message,
       bool IsRead,
       DateTime CreatedAt
   );

    public record CreateNotification(
        int UserId,
        string Title,
        string Message
    );

    public record UpdateNotification(
        int Id,
        string? Title,
        string? Message,
        bool? IsRead
    );
}
