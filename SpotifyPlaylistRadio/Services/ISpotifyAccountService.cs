using SpotifyPlaylistRadio.Models;

namespace SpotifyPlaylistRadio.Services
{
    public interface ISpotifyAccountService
    {
        Task RefreshToken(string refreshToken, string clientId, string clientSecret);
    }
}
