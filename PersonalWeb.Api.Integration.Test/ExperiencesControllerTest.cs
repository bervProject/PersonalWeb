using System;
using Microsoft.AspNetCore.Mvc.Testing;
using PersonalWeb.Api.Integration.Test.Models;
using PersonalWeb.Api.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace PersonalWeb.Api.Integration.Test
{
    public class ExperiencesControllerTest : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly string _baseUrl = "/v1/Experiences";
        private readonly EntityFramework.PersonalWebApiContext _dbContext;
        private readonly IServiceScope _scope;
        private readonly HttpClient _httpClient;

        public ExperiencesControllerTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient();
            _scope = _factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<EntityFramework.PersonalWebApiContext>();
        }
        
        public void Dispose()
        {
            _dbContext.Experiences.ExecuteDelete();
            _dbContext.Dispose();
            _scope.Dispose();
        }

        [Fact]
        public async Task Get()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(_baseUrl);
            Assert.True(response.IsSuccessStatusCode);
            var bodyResponse = await response.Content.ReadFromJsonAsync<ODataListResponse<Experience>>();
            var emptyResult = new List<Experience>();
            Assert.NotNull(bodyResponse);
            Assert.Equal(emptyResult, bodyResponse.Value);
        }
        
        [Fact]
        public async Task PostTest()
        {
            var newProject = new Experience()
            {
                Title = "My Experience",
                Description = "Hello My Experience",
                StartDate = new DateTimeOffset(),
                Location = "Bandung",
                ExperienceType = "Work",
                EntityName = "My Work",
                EntityLink = "https://personal.com",
                CreateBy = "Test",
                CreatedDate = new DateTimeOffset()
            };
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, newProject);
            Assert.True(response.IsSuccessStatusCode);
            var bodyResponse = await response.Content.ReadFromJsonAsync<Experience>();
            Assert.NotNull(bodyResponse);
            Assert.Equal("My Experience", bodyResponse.Title);
            Assert.Equal("Hello My Experience", bodyResponse.Description);
            Assert.Equal("Bandung", bodyResponse.Location);
            Assert.Equal("Work", bodyResponse.ExperienceType);
            Assert.Equal("My Work", bodyResponse.EntityName);
            Assert.Equal("https://personal.com", bodyResponse.EntityLink);

        }
    }
}
