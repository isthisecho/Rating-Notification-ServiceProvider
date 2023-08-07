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

        [HttpGet("{id}", Name = "GetNewNotifications")]
        public IActionResult GetNewNotifications(int id)
        {
            try
            {
                IEnumerable<NotificationDTO> notifications = _notificationService.GetAllNewNotifications(id);

                _logger.LogInformation("New notifications retrieved successfully for ServiceProvider Id {id}  : \r\n {@notifications}",id, notifications);

                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting new notifications");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}
