using FintechApp.Application.DTOs;
using FintechApp.Application.Interfaces;
using FintechApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FintechApp.Presentation.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _service;
        public NotificationController(INotificationService service)
        {
            _service = service;
        }
        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<List<Notification>>> GetNotifications(int userId, int pageNumber = 1, int pageSize = 20)
        {
            var notifications = await _service.GetNotificationsAsync(userId, pageNumber = 1, pageSize = 20);
            return Ok(notifications);
        }
        [HttpGet("user/{userId:int}/unread")]
        public async Task<ActionResult<List<Notification>>> GetUnreadNotifications(int userId)
        {
            var notifications = await _service.GetUnreadNotificationsAsync(userId);
            return Ok(notifications);
        }
        [HttpPost]
        public async Task<ActionResult<Notification>> CreateNotification([FromBody] CreateNotification request)
        {
            var notif = await _service.CreateNotificationAsync(request.UserId, request.Title, request.Message);
            return Ok(notif);
        }
        [HttpPut("{id:int}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _service.MarkAsReadAsync(id);
            return NoContent();
        }

        // PUT: api/notification/user/?/read-all
        [HttpPut("user/{userId:int}/read-all")]
        public async Task<IActionResult> MarkAllAsRead(int userId)
        {
            await _service.MarkAllAsReadAsync(userId);
            return NoContent();
        }
    }
}
