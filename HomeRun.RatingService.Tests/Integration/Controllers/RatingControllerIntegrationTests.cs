using FakeItEasy;
using HomeRun.RatingService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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


            _client.Dispose();
            _factory.Dispose();
        }




    }
}
