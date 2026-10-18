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
    public class ProfilesControllerTest : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly string _baseUrl = "/v1/Profiles";
        private readonly EntityFramework.PersonalWebApiContext _dbContext;
        private readonly IServiceScope _scope;
        private readonly HttpClient _httpClient;

        public ProfilesControllerTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient();
            _scope = _factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<EntityFramework.PersonalWebApiContext>();
        }

        public void Dispose()
        {
            _dbContext.Profiles.ExecuteDelete();
            _dbContext.Dispose();
            _scope.Dispose();
        }

        [Fact]
        public async Task GetTest()
        {
            var response = await _httpClient.GetAsync(_baseUrl);
            Assert.True(response.IsSuccessStatusCode);
            var bodyResponse = await response.Content.ReadFromJsonAsync<ODataListResponse<Profile>>();
            Assert.NotNull(bodyResponse);
            Assert.IsType<List<Profile>>(bodyResponse.Value);
        }

        [Fact]
        public async Task PostTest()
        {
            await AuthHelper.AuthenticateAsync(_httpClient);
            var newProfile = new Profile
            {
                FullName = "Bervianto Leo",
                Titles = "[\"Software Engineer\",\"AWS Community Builder\"]",
                Bio = "A passionate engineer.",
                CreateBy = "Test",
                CreatedDate = new DateTimeOffset()
            };
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, newProfile);
            Assert.True(response.IsSuccessStatusCode);
            var bodyResponse = await response.Content.ReadFromJsonAsync<Profile>();
            Assert.NotNull(bodyResponse);
            Assert.Equal("Bervianto Leo", bodyResponse.FullName);
            Assert.Equal("[\"Software Engineer\",\"AWS Community Builder\"]", bodyResponse.Titles);
            Assert.NotEqual(Guid.Empty, bodyResponse.Id);
        }

        [Fact]
        public async Task GetByIdTest()
        {
            await AuthHelper.AuthenticateAsync(_httpClient);
            var newProfile = new Profile
            {
                FullName = "Bervianto Leo",
                Titles = "[\"Software Engineer\"]",
                CreateBy = "Test",
                CreatedDate = new DateTimeOffset()
            };
            var createResponse = await _httpClient.PostAsJsonAsync(_baseUrl, newProfile);
            Assert.True(createResponse.IsSuccessStatusCode);
            var created = await createResponse.Content.ReadFromJsonAsync<Profile>();
            Assert.NotNull(created);

            var getResponse = await _httpClient.GetAsync($"{_baseUrl}/{created.Id}");
            Assert.True(getResponse.IsSuccessStatusCode);
            var bodyResponse = await getResponse.Content.ReadFromJsonAsync<Profile>();
            Assert.NotNull(bodyResponse);
            Assert.Equal(created.Id, bodyResponse.Id);
            Assert.Equal("Bervianto Leo", bodyResponse.FullName);
        }

        [Fact]
        public async Task PatchTest()
        {
            await AuthHelper.AuthenticateAsync(_httpClient);
            var newProfile = new Profile
            {
                FullName = "Bervianto Leo",
                Titles = "[\"Software Engineer\"]",
                CreateBy = "Test",
                CreatedDate = new DateTimeOffset()
            };
            var createResponse = await _httpClient.PostAsJsonAsync(_baseUrl, newProfile);
            Assert.True(createResponse.IsSuccessStatusCode);
            var created = await createResponse.Content.ReadFromJsonAsync<Profile>();
            Assert.NotNull(created);

            created.FullName = "Leo Updated";
            var patchResponse = await _httpClient.PatchAsJsonAsync($"{_baseUrl}/{created.Id}", created);
            Assert.True(patchResponse.IsSuccessStatusCode);
        }

        [Fact]
        public async Task DeleteTest()
        {
            await AuthHelper.AuthenticateAsync(_httpClient);
            var newProfile = new Profile
            {
                FullName = "Bervianto Leo",
                Titles = "[\"Software Engineer\"]",
                CreateBy = "Test",
                CreatedDate = new DateTimeOffset()
            };
            var createResponse = await _httpClient.PostAsJsonAsync(_baseUrl, newProfile);
            Assert.True(createResponse.IsSuccessStatusCode);
            var created = await createResponse.Content.ReadFromJsonAsync<Profile>();
            Assert.NotNull(created);

            var deleteResponse = await _httpClient.DeleteAsync($"{_baseUrl}/{created.Id}");
            Assert.True(deleteResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }
    }
}
