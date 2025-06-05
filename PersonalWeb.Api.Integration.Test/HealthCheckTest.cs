using Microsoft.AspNetCore.Mvc.Testing;
using PersonalWeb.Api.Integration.Test.Models;
using PersonalWeb.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PersonalWeb.Api.Integration.Test
{
    public class HealthCheckTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly string _baseUrl = "/health";

        public HealthCheckTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(_baseUrl);
            Assert.True(response.IsSuccessStatusCode);
            var bodyResponse = await response.Content.ReadAsStringAsync();
            Assert.Equal("Healthy", bodyResponse);
        }
    }
}
