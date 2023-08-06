using FakeItEasy;
using HomeRun.RatingService.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using HomeRun.Shared.Helpers;
using System.Threading.Tasks;

namespace HomeRun.RatingService.Tests.Integration.Controllers
{
    public class RatingControllerIntegrationTests : IClassFixture<RatingApiFactory>
    {
        private readonly RatingApiFactory _factory;
        private readonly HttpClient _client;


        public RatingControllerIntegrationTests(RatingApiFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task OnSubmitRating_WhenExecuteController_ShouldReturnExcpectedResult()
        {
            RatingDTO ratingDto = new RatingDTO() {  RatingValue = 5, ServiceProviderId =1};


            var request = await _client.PostAsync(HttpHelper.Urls.SubmitRating, HttpHelper.GetJsonHttpContent(ratingDto));
            var responseContent = await request.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, request.StatusCode);

            var responseObject = JsonConvert.DeserializeObject<CommonResponse>(responseContent);
            Assert.True(responseObject?.IsSuccess);
            Assert.Equal("Rating created successfully", responseObject?.Message);


        }


        [Theory]
        [InlineData(4,3)]
        [InlineData(5, 2)]
        public async Task OnSubmitRatingWithInvalidProviderId_WhenExecuteController_ShouldReturnException(int serviceProivderId, int ratingValue) // I have only added 3 service providers  and their id's : 1,2,3  
        {
            RatingDTO ratingDto = new RatingDTO() { ServiceProviderId = serviceProivderId, RatingValue = ratingValue };


            var response = await _client.PostAsync(HttpHelper.Urls.SubmitRating, HttpHelper.GetJsonHttpContent(ratingDto));
            var responseContent = await response.Content.ReadAsStringAsync();


            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // Expecting a BadRequest response status code

            var responseObject = JsonConvert.DeserializeObject<ProblemDetails>(responseContent);
            Assert.Equal("No Service Provider exists with specified id.", responseObject?.Detail);

        }

        [Theory]
        [InlineData(1, 6)]
        [InlineData(2, -3)]
        public async Task OnSubmitRatinWithInvalidRating_WhenExecuteController_ShouldReturnValidationProblem(int serviceProivderId, int ratingValue)  // Rating Value must be between 0-5
        {
            RatingDTO ratingDto = new RatingDTO() { ServiceProviderId = serviceProivderId, RatingValue = ratingValue };


            var response = await _client.PostAsync(HttpHelper.Urls.SubmitRating, HttpHelper.GetJsonHttpContent(ratingDto));
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // Expecting a BadRequest response status code

        }






    }
}
