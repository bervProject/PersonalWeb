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
    public class PublicationsControllerTest : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly string _baseUrl = "/v1/Publications";
        private readonly EntityFramework.PersonalWebApiContext _dbContext;
        private readonly IServiceScope _scope;
        private readonly HttpClient _httpClient;

        public PublicationsControllerTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient();
            _scope = _factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<EntityFramework.PersonalWebApiContext>();
        }

        public void Dispose()
        {
            _dbContext.Publications.ExecuteDelete();
            _dbContext.Dispose();
            _scope.Dispose();
        }

        [Fact]
        public async Task GetTest()
        {
            var response = await _httpClient.GetAsync(_baseUrl);
            Assert.True(response.IsSuccessStatusCode);
            var bodyResponse = await response.Content.ReadFromJsonAsync<ODataListResponse<Publication>>();
            Assert.NotNull(bodyResponse);
            Assert.IsType<List<Publication>>(bodyResponse.Value);
        }

        [Fact]
        public async Task PostTest()
        {
            await AuthHelper.AuthenticateAsync(_httpClient);
            var newPublication = new Publication
            {
                Title = "Getting Started with AWS CDK",
                Slug = "getting-started-aws-cdk",
                Summary = "A beginner-friendly guide to AWS CDK.",
                Source = "dev.to",
                ExternalUrl = "https://dev.to/bervianto/getting-started-aws-cdk",
                PublishedDate = new DateTimeOffset(2024, 3, 10, 0, 0, 0, TimeSpan.Zero),
                CreateBy = "Test",
                CreatedDate = new DateTimeOffset()
            };
            var response = await _httpClient.PostAsJsonAsync(_baseUrl, newPublication);
            Assert.True(response.IsSuccessStatusCode);
            var bodyResponse = await response.Content.ReadFromJsonAsync<Publication>();
            Assert.NotNull(bodyResponse);
            Assert.Equal("Getting Started with AWS CDK", bodyResponse.Title);
            Assert.Equal("getting-started-aws-cdk", bodyResponse.Slug);
            Assert.Equal("dev.to", bodyResponse.Source);
            Assert.NotEqual(Guid.Empty, bodyResponse.Id);
        }

        [Fact]
        public async Task GetByIdTest()
        {
            await AuthHelper.AuthenticateAsync(_httpClient);
            var newPublication = new Publication
            {
                Title = "IBM Champion Tips",
                Slug = "ibm-champion-tips",
                Source = "native",
                CreateBy = "Test",
                CreatedDate = new DateTimeOffset()
            };
            var createResponse = await _httpClient.PostAsJsonAsync(_baseUrl, newPublication);
            Assert.True(createResponse.IsSuccessStatusCode);
            var created = await createResponse.Content.ReadFromJsonAsync<Publication>();
            Assert.NotNull(created);

            var getResponse = await _httpClient.GetAsync($"{_baseUrl}/{created.Id}");
            Assert.True(getResponse.IsSuccessStatusCode);
            var bodyResponse = await getResponse.Content.ReadFromJsonAsync<Publication>();
            Assert.NotNull(bodyResponse);
            Assert.Equal(created.Id, bodyResponse.Id);
            Assert.Equal("IBM Champion Tips", bodyResponse.Title);
        }

        [Fact]
        public async Task PatchTest()
        {
            await AuthHelper.AuthenticateAsync(_httpClient);
            var newPublication = new Publication
            {
                Title = "HashiCorp Terraform Intro",
                Slug = "hashicorp-terraform-intro",
                Source = "dev.to",
                CreateBy = "Test",
                CreatedDate = new DateTimeOffset()
            };
            var createResponse = await _httpClient.PostAsJsonAsync(_baseUrl, newPublication);
            Assert.True(createResponse.IsSuccessStatusCode);
            var created = await createResponse.Content.ReadFromJsonAsync<Publication>();
            Assert.NotNull(created);

            created.Title = "HashiCorp Terraform Intro (Updated)";
            var patchResponse = await _httpClient.PatchAsJsonAsync($"{_baseUrl}/{created.Id}", created);
            Assert.True(patchResponse.IsSuccessStatusCode);
        }

        [Fact]
        public async Task DeleteTest()
        {
            await AuthHelper.AuthenticateAsync(_httpClient);
            var newPublication = new Publication
            {
                Title = "To Be Deleted Post",
                Slug = "to-be-deleted",
                Source = "native",
                CreateBy = "Test",
                CreatedDate = new DateTimeOffset()
            };
            var createResponse = await _httpClient.PostAsJsonAsync(_baseUrl, newPublication);
            Assert.True(createResponse.IsSuccessStatusCode);
            var created = await createResponse.Content.ReadFromJsonAsync<Publication>();
            Assert.NotNull(created);

            var deleteResponse = await _httpClient.DeleteAsync($"{_baseUrl}/{created.Id}");
            Assert.True(deleteResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }
    }
}
