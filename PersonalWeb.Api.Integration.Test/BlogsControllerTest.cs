using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PersonalWeb.Api.Integration.Test.Models;
using PersonalWeb.Api.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace PersonalWeb.Api.Integration.Test
{
    public class BlogsControllerTest : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly string _baseUrl = "/v1/Blogs";
        private readonly EntityFramework.PersonalWebApiContext _dbContext;
        private readonly IServiceScope _scope;
        private readonly HttpClient _httpClient;

        public BlogsControllerTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient();
            _scope = _factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<EntityFramework.PersonalWebApiContext>();
        }

        public void Dispose()
        {
            _dbContext.Blogs.ExecuteDelete();
            _dbContext.Dispose();
            _scope.Dispose();
        }

        [Fact]
        public async Task GetTest()
        {
            var response = await _httpClient.GetAsync(_baseUrl);
            Assert.True(response.IsSuccessStatusCode);
            var bodyResponse = await response.Content.ReadFromJsonAsync<ODataListResponse<Blog>>();
            var emptyResult = new List<Blog>();
            Assert.NotNull(bodyResponse);
            Assert.Equal(emptyResult, bodyResponse.Value);
        }

        [Fact]
        public async Task PostTest()
        {
            var newBlog = new Blog
            {
                Title = "My Blog",
                Description = "Hello My Blog",
                Icon = "b-icon",
                Link = "https://test.com",
                CreateBy = "Test",
                CreatedDate = new DateTimeOffset()
            };
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, newBlog);
            Assert.True(response.IsSuccessStatusCode);
            var bodyResponse = await response.Content.ReadFromJsonAsync<Blog>();
            Assert.NotNull(bodyResponse);
            Assert.Equal("My Blog", bodyResponse.Title);
            Assert.Equal("Hello My Blog", bodyResponse.Description);
            Assert.Equal("b-icon", bodyResponse.Icon);
            Assert.Equal("https://test.com", bodyResponse.Link);
        }
        
        [Fact]
        public async Task PostInvalidTest()
        {
            var newBlog = new Blog
            {
                Title = "My Blog",
                Description = "Hello My Blog",
                Icon = "b-icon",
                Link = "https://test.com",
            };
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, newBlog);
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        
        [Fact]
        public async Task GetByIdTest()
        {
            var newBlog = new Blog
            {
                Title = "My Blog",
                Description = "Hello My Blog",
                Icon = "b-icon",
                Link = "https://test.com",
                CreateBy = "Test",
                CreatedDate = new DateTimeOffset()
            };
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, newBlog);
            Assert.True(response.IsSuccessStatusCode);
            var bodyResponse = await response.Content.ReadFromJsonAsync<Blog>();
            Assert.NotNull(bodyResponse);
            Assert.Equal("My Blog", bodyResponse.Title);
            Assert.Equal("Hello My Blog", bodyResponse.Description);
            Assert.Equal("b-icon", bodyResponse.Icon);
            Assert.Equal("https://test.com", bodyResponse.Link);
            Assert.NotEqual(Guid.Empty, bodyResponse.Id);
            newBlog.Id = bodyResponse.Id;
            newBlog.Description = "Updated";
            response = await _httpClient.GetAsync($"{_baseUrl}/{bodyResponse.Id}");
            Assert.True(response.IsSuccessStatusCode);
            bodyResponse = await response.Content.ReadFromJsonAsync<Blog>();
            Assert.NotNull(bodyResponse);
            Assert.Equal("My Blog", bodyResponse.Title);
            Assert.Equal("Hello My Blog", bodyResponse.Description);
            Assert.Equal("b-icon", bodyResponse.Icon);
            Assert.Equal("https://test.com", bodyResponse.Link);
            Assert.NotEqual(Guid.Empty, bodyResponse.Id);
        }
        
        [Fact]
        public async Task PatchTest()
        {
            var newBlog = new Blog
            {
                Title = "My Blog",
                Description = "Hello My Blog",
                Icon = "b-icon",
                Link = "https://test.com",
                CreateBy = "Test",
                CreatedDate = new DateTimeOffset()
            };
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, newBlog);
            Assert.True(response.IsSuccessStatusCode);
            var bodyResponse = await response.Content.ReadFromJsonAsync<Blog>();
            Assert.NotNull(bodyResponse);
            Assert.Equal("My Blog", bodyResponse.Title);
            Assert.Equal("Hello My Blog", bodyResponse.Description);
            Assert.Equal("b-icon", bodyResponse.Icon);
            Assert.Equal("https://test.com", bodyResponse.Link);
            Assert.NotEqual(Guid.Empty, bodyResponse.Id);
            newBlog.Id = bodyResponse.Id;
            newBlog.Description = "Updated";
            response = await _httpClient.PatchAsJsonAsync($"{_baseUrl}/{bodyResponse.Id}", newBlog);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
        
        [Fact]
        public async Task DeleteTest()
        {
            var newBlog = new Blog
            {
                Title = "My Blog",
                Description = "Hello My Blog",
                Icon = "b-icon",
                Link = "https://test.com",
                CreateBy = "Test",
                CreatedDate = new DateTimeOffset()
            };
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, newBlog);
            Assert.True(response.IsSuccessStatusCode);
            var bodyResponse = await response.Content.ReadFromJsonAsync<Blog>();
            Assert.NotNull(bodyResponse);
            Assert.Equal("My Blog", bodyResponse.Title);
            Assert.Equal("Hello My Blog", bodyResponse.Description);
            Assert.Equal("b-icon", bodyResponse.Icon);
            Assert.Equal("https://test.com", bodyResponse.Link);
            Assert.NotEqual(Guid.Empty, bodyResponse.Id);
            response = await _httpClient.DeleteAsync($"{_baseUrl}/{bodyResponse.Id}");
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}