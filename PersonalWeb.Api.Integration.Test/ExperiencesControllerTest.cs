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
            var response = await _httpClient.GetAsync(_baseUrl);
            Assert.True(response.IsSuccessStatusCode);
            var bodyResponse = await response.Content.ReadFromJsonAsync<ODataListResponse<Experience>>();
            var emptyResult = new List<Experience>();
            Assert.NotNull(bodyResponse);
            Assert.Equal(emptyResult, bodyResponse.Value);
        }

        [Fact]
        public async Task PostTest()
        {
            await AuthHelper.AuthenticateAsync(_httpClient);
            var newExperience = new Experience
            {
                CompanyName = "Acme Corp",
                RoleTitle = "Software Engineer",
                Location = "Bandung",
                StartDate = new DateTimeOffset(2022, 1, 1, 0, 0, 0, TimeSpan.Zero),
                IsCurrent = true,
                CreateBy = "Test",
                CreatedDate = new DateTimeOffset()
            };
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, newExperience);
            Assert.True(response.IsSuccessStatusCode);
            var bodyResponse = await response.Content.ReadFromJsonAsync<Experience>();
            Assert.NotNull(bodyResponse);
            Assert.Equal("Acme Corp", bodyResponse.CompanyName);
            Assert.Equal("Software Engineer", bodyResponse.RoleTitle);
            Assert.Equal("Bandung", bodyResponse.Location);
            Assert.True(bodyResponse.IsCurrent);
        }
    }
}
