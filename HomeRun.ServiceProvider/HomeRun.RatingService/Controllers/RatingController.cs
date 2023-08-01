using Microsoft.AspNetCore.Mvc;

namespace HomeRun.RatingService
{
    [ApiController]
    [Route("[controller]")]
    public class RatingController : ControllerBase
    {

        private readonly ILogger<RatingController> _logger;
        private readonly IRatingService _ratingService;

        public RatingController(ILogger<RatingController> logger,IRatingService ratingService)
        {
            _logger = logger;
            _ratingService = ratingService;
        }


        [HttpPost(Name = "SubmitRating")]
        public IActionResult SubmitRating(RatingDTO rating)
        {
            Task<Rating> x = _ratingService.SubmitRating(rating);
            if(x is not null)
            {
                   return Ok(x);
            }

            return BadRequest();
        }

        [HttpGet(Name = "GetAverageRating")]
        public IActionResult GetAvreageRating(int serviceProviderId)
        {
           var x =  _ratingService.GetAverageRating(serviceProviderId).Result;
            return Ok();
        }


    }


}
