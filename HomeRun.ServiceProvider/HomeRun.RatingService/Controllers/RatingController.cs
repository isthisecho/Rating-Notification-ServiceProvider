using HomeRun.RatingService.Models;
using HomeRun.RatingService.Models.DTO_s;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HomeRun.RatingService
{
    [ApiController]
    [Route("[controller]")]
    [EnableRateLimiting("fixed")]
    public class RatingController : ControllerBase
    {

        private readonly ILogger<RatingController> _logger;
        private readonly IRatingService     _ratingService;
        private readonly IMessageProducer _messageProducer;

        public RatingController(ILogger<RatingController> logger,IRatingService ratingService, IMessageProducer messageProducer)
        {
            _logger        = logger;
            _ratingService = ratingService;
            _messageProducer = messageProducer;
        }


        [HttpPost(Name = "SubmitRating")]
        public async Task<IActionResult> SubmitRating(RatingDTO rating)
        {
            if (ModelState.IsValid)
            {
                Rating _rating = await _ratingService.SubmitRating(rating);
                CommonResponse response = new() { IsSuccess = true, Message = "Rating created successfully", Result = _rating };

                _logger.LogInformation("Rating created successfully: {@_rating}", _rating); // Rating Value logs.  

                NotificationDTO notification = new NotificationDTO()
                {
                    RatingId          = _rating.Id,
                    RatingValue       = _rating.RatingValue,
                    CreatedAt         = _rating.CreatedAt,
                    ServiceProviderId = _rating.ServiceProviderId  
                };

                _messageProducer.SendingMessage(notification);                                   // Send notification to MQ.

                return Ok(response);
            }

            return BadRequest("sa");
        }

        [HttpGet("GetAvg/{id}", Name = "GetAverageRating")]
        public async Task<IActionResult> GetAverageRating(int id)
        {
            double value = await _ratingService.GetAverageRating(id);
            CommonResponse response = new CommonResponse() { IsSuccess = true  , Result =value , Message = $"Average Rating is: {value}" };

            // Rating Value Logs
            _logger.LogInformation("Average rating calculated: {value}", value);

            return Ok(response);
        }

    }


}
