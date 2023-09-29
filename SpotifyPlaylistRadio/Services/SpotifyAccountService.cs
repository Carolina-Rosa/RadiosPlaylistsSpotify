using SpotifyPlaylistRadio.Models;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;


namespace SpotifyPlaylistRadio.Services
{
    public class SpotifyAccountService : ISpotifyAccountService
    {
        private readonly HttpClient _httpClient;

        public SpotifyAccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AuthToken> RefreshToken(string refreshToken, string clientId, string clientSecret)
        {
            HttpRequestMessage req = new(HttpMethod.Post, "token");
            req.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")));
            req.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken }            
            });

            HttpResponseMessage response = await _httpClient.SendAsync(req);

            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AuthToken>(jsonString);
            }
            return null;
        }
    }
}
