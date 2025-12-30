using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolProcurement.Api.Service.Interface;

namespace SchoolProcurement.Api.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationsController(INotificationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            int page = 1,
            int pageSize = 10,
            CancellationToken ct = default)
            => Ok(await _service.GetPagedAsync(page, pageSize, ct));

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount(CancellationToken ct)
            => Ok(await _service.GetUnreadCountAsync(ct));

        [HttpPost("mark-read/{id}")]
        public async Task<IActionResult> MarkRead(int id, CancellationToken ct)
            => Ok(await _service.MarkAsReadAsync(id, ct));

        [HttpPost("mark-all-read")]
        public async Task<IActionResult> MarkAllRead(CancellationToken ct)
            => Ok(await _service.MarkAllAsReadAsync(ct));
    }
}
