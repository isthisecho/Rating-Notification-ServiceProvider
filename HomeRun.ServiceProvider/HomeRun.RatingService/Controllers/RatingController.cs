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

            try
            {
                Rating? _rating = await _ratingService.SubmitRating(rating);

                if (_rating is not null)
                {
                    response.IsSuccess = true;
                    response.Result = _rating;
                    response.Message = "Rating created successfully";

                    // Rating Value logs.
                    _logger.LogInformation("Rating created successfully: {@_rating}", _rating);

                    // Send rating to MQ.
                    _messageProducer.SendingMessage(_rating);

                    return Ok(response);
                }

                response.IsSuccess = false;
                response.Result = _rating;
                response.Message = $"Rating could not be added";

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                //Log in case of errors
                _logger.LogError("An error occurred while submitting the rating: {ex}", ex);

                //Throwing exception for carrying errors to the upper layers. 
                throw;
            }
        }

        [HttpGet("GetAvgRating", Name = "GetAverageRating")]
        public async Task<IActionResult> GetAverageRating(int serviceProviderId)
        {
            CommonResponse response = new CommonResponse();

            try
            {
                double value = await _ratingService.GetAverageRating(serviceProviderId);

                if (value >= 0)
                {
                    response.IsSuccess = true;
                    response.Result = value;
                    response.Message = $"Average Rating is: {value}";

                    // Rating Value Logs
                    _logger.LogInformation("Average rating calculated: {value}", value);

                    return Ok(response);
                }

                response.IsSuccess = false;
                response.Result = value;
                response.Message = $"Average rating cannot be computed";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                //Log in case of errors
                _logger.LogError("An error occurred while calculating the average rating: {ex.Message}" , ex.Message);

                //Throwing exception for carrying errors to the upper layers. 
                throw;
            }
        }

    }


}
