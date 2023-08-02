using Microsoft.AspNetCore.Mvc;

namespace HomeRun.NotificationService
{

    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(ILogger<NotificationController> logger, INotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }


        [HttpGet("GetNotifications", Name = "GetNewNotifications")]
        public  IActionResult GetNewNotifications(int serviceProviderId)
        {
            IEnumerable<Notification> x = _notificationService.GetAllNewNotifications();
            return Ok(x);
        }

    }
}
