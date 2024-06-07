using SpotifyPlaylistRadio.Models;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;


namespace SpotifyPlaylistRadio.Services
{
    public class SpotifyAccountService : ISpotifyAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthToken _authToken;

        public SpotifyAccountService(HttpClient httpClient, AuthToken authToken)
        {
            _httpClient = httpClient;
            _authToken = authToken;
        }

        public async Task RefreshToken(string refreshToken, string clientId, string clientSecret)
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
                var tokenResponse = JsonConvert.DeserializeObject<AuthToken>(jsonString);

                _authToken.access_token = tokenResponse.access_token;
                _authToken.token_type= tokenResponse.token_type;
                _authToken.expires_in = tokenResponse.expires_in;
            }
            
        }
    }
}
