using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PersonalWeb.Api.Integration.Test
{
    public static class AuthHelper
    {
        private const string LoginUrl = "/api/auth/login";
        private const string DefaultEmail = "admin@personalweb.local";
        private const string DefaultPassword = "Change_Me_123!";

        public static async Task<string> GetAccessTokenAsync(HttpClient client)
        {
            var response = await client.PostAsJsonAsync(LoginUrl, new
            {
                email = DefaultEmail,
                password = DefaultPassword
            });
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return body!.AccessToken;
        }

        public static async Task AuthenticateAsync(HttpClient client)
        {
            var token = await GetAccessTokenAsync(client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private class LoginResponse
        {
            public string AccessToken { get; set; } = string.Empty;
        }
    }
}
