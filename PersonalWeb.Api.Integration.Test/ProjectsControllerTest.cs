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
    public class ProjectsControllerTest : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly string _baseUrl = "/v1/Projects";
        private readonly EntityFramework.PersonalWebApiContext _dbContext;
        private readonly IServiceScope _scope;
        private readonly HttpClient _httpClient;

        public ProjectsControllerTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient();
            _scope = _factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<EntityFramework.PersonalWebApiContext>();
        }

        public void Dispose()
        {
            _dbContext.Projects.ExecuteDelete();
            _dbContext.Dispose();
            _scope.Dispose();
        }

        [Fact]
        public async Task Get()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(_baseUrl);
            Assert.True(response.IsSuccessStatusCode);
            var bodyResponse = await response.Content.ReadFromJsonAsync<ODataListResponse<Project>>();
            var emptyResult = new List<Project>();
            Assert.NotNull(bodyResponse);
            Assert.Equal(emptyResult, bodyResponse.Value);
        }

        [Fact]
        public async Task PostTest()
        {
            var newProject = new Project()
            {
                Title = "My Project",
                Description = "Hello My Project",
                Link = "https://test.com",
                CreateBy = "Test",
                CreatedDate = new DateTimeOffset()
            };
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, newProject);
            Assert.True(response.IsSuccessStatusCode);
            var bodyResponse = await response.Content.ReadFromJsonAsync<Project>();
            Assert.NotNull(bodyResponse);
            Assert.Equal("My Project", bodyResponse.Title);
            Assert.Equal("Hello My Project", bodyResponse.Description);
            Assert.Equal("https://test.com", bodyResponse.Link);
        }
    }
}