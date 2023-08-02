using HomeRun.RatingService.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeRun.RatingService
{
    [ApiController]
    [Route("[controller]")]
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


        [HttpPost("SubmitRating", Name = "SubmitRating")]
        public async Task<IActionResult> SubmitRating(RatingDTO rating)
        {
            CommonResponse response = new CommonResponse();

            Rating? _rating = await _ratingService.SubmitRating(rating);

            if(_rating is not null)
            {
                response.IsSuccess = true;
                response.Result = _rating;
                response.Message = $"Rating created successfully : {response.Result}";

                _messageProducer.SendingMessage(_rating);
                _logger.LogInformation(message: response.Message);

                return Ok(response);
            }
            
            response.IsSuccess = false;
            response.Result = _rating;
            response.Message = $"Rating can not be added";
            _logger.LogError(
                "Request Failure BURAYA BAKKKK {@RequestName}, {@Error}, {@DateTimeUTC}"
                ,typeof(string),
                "asda",
                DateTime.UtcNow
                );
            return BadRequest(response);
        }

        [HttpGet("GetAvgRating", Name = "GetAverageRating")]
        public async Task<IActionResult> GetAverageRating(int serviceProviderId)
        {
            CommonResponse response = new CommonResponse();

            double value = await _ratingService.GetAverageRating(serviceProviderId);

            if(value >= 0)
            {
                response.IsSuccess = true;
                response.Result = value;
                response.Message = $"Average Rating is :{value}";

                return Ok(response);
            }

            response.IsSuccess = false;
            response.Result = value;
            response.Message = $"Average rating can not be computed";
            return BadRequest(response);    

        }

    }


}
