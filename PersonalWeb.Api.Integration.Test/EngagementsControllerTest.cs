using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersonalWeb.Api.Integration.Test.Models;
using PersonalWeb.Api.Models;
using Xunit;

namespace PersonalWeb.Api.Integration.Test
{
    public class EngagementsControllerTest : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly string _baseUrl = "/v1/Engagements";
        private readonly EntityFramework.PersonalWebApiContext _dbContext;
        private readonly IServiceScope _scope;
        private readonly HttpClient _httpClient;

        public EngagementsControllerTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient();
            _scope = _factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<EntityFramework.PersonalWebApiContext>();
        }

        public void Dispose()
        {
            _dbContext.Engagements.ExecuteDelete();
            _dbContext.Dispose();
            _scope.Dispose();
        }

        [Fact]
        public async Task GetTest()
        {
            var response = await _httpClient.GetAsync(_baseUrl);
            Assert.True(response.IsSuccessStatusCode);
            var bodyResponse = await response.Content.ReadFromJsonAsync<ODataListResponse<Engagement>>();
            Assert.NotNull(bodyResponse);
            Assert.IsType<List<Engagement>>(bodyResponse.Value);
        }

        [Fact]
        public async Task PostTest()
        {
            await AuthHelper.AuthenticateAsync(_httpClient);
            var newEngagement = new Engagement
            {
                EventName = "AWS Summit Jakarta",
                EngagementType = "Speaker",
                TopicOrRole = "Cloud-Native .NET",
                EventDate = new DateTimeOffset(2024, 6, 15, 0, 0, 0, TimeSpan.Zero),
                Location = "Jakarta",
                CreateBy = "Test",
                CreatedDate = new DateTimeOffset()
            };
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, newEngagement);
            Assert.True(response.IsSuccessStatusCode);
            var bodyResponse = await response.Content.ReadFromJsonAsync<Engagement>();
            Assert.NotNull(bodyResponse);
            Assert.Equal("AWS Summit Jakarta", bodyResponse.EventName);
            Assert.Equal("Speaker", bodyResponse.EngagementType);
            Assert.Equal("Jakarta", bodyResponse.Location);
            Assert.NotEqual(Guid.Empty, bodyResponse.Id);
        }

        [Fact]
        public async Task GetByIdTest()
        {
            await AuthHelper.AuthenticateAsync(_httpClient);
            var newEngagement = new Engagement
            {
                EventName = "TechXchange",
                EngagementType = "Mentor",
                EventDate = new DateTimeOffset(2024, 9, 10, 0, 0, 0, TimeSpan.Zero),
                CreateBy = "Test",
                CreatedDate = new DateTimeOffset()
            };
            var createResponse = await _httpClient.PostAsJsonAsync(_baseUrl, newEngagement);
            Assert.True(createResponse.IsSuccessStatusCode);
            var created = await createResponse.Content.ReadFromJsonAsync<Engagement>();
            Assert.NotNull(created);

            var getResponse = await _httpClient.GetAsync($"{_baseUrl}/{created.Id}");
            Assert.True(getResponse.IsSuccessStatusCode);
            var bodyResponse = await getResponse.Content.ReadFromJsonAsync<Engagement>();
            Assert.NotNull(bodyResponse);
            Assert.Equal(created.Id, bodyResponse.Id);
            Assert.Equal("TechXchange", bodyResponse.EventName);
        }

        [Fact]
        public async Task PatchTest()
        {
            await AuthHelper.AuthenticateAsync(_httpClient);
            var newEngagement = new Engagement
            {
                EventName = "Dicoding Bootcamp",
                EngagementType = "Instructor",
                EventDate = new DateTimeOffset(2023, 3, 5, 0, 0, 0, TimeSpan.Zero),
                CreateBy = "Test",
                CreatedDate = new DateTimeOffset()
            };
            var createResponse = await _httpClient.PostAsJsonAsync(_baseUrl, newEngagement);
            Assert.True(createResponse.IsSuccessStatusCode);
            var created = await createResponse.Content.ReadFromJsonAsync<Engagement>();
            Assert.NotNull(created);

            created.EventName = "Dicoding Bootcamp Updated";
            var patchResponse = await _httpClient.PatchAsJsonAsync($"{_baseUrl}/{created.Id}", created);
            Assert.True(patchResponse.IsSuccessStatusCode);
        }

        [Fact]
        public async Task DeleteTest()
        {
            await AuthHelper.AuthenticateAsync(_httpClient);
            var newEngagement = new Engagement
            {
                EventName = "Google Summer of Code",
                EngagementType = "Mentor",
                EventDate = new DateTimeOffset(2023, 5, 1, 0, 0, 0, TimeSpan.Zero),
                CreateBy = "Test",
                CreatedDate = new DateTimeOffset()
            };
            var createResponse = await _httpClient.PostAsJsonAsync(_baseUrl, newEngagement);
            Assert.True(createResponse.IsSuccessStatusCode);
            var created = await createResponse.Content.ReadFromJsonAsync<Engagement>();
            Assert.NotNull(created);

            var deleteResponse = await _httpClient.DeleteAsync($"{_baseUrl}/{created.Id}");
            Assert.True(deleteResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }
    }
}
