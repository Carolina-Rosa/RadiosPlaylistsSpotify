using SpotifyPlaylistRadio.Models;

namespace SpotifyPlaylistRadio.Services
{
    public interface ISpotifyAccountService
    {
        Task<AuthToken> RefreshToken(string refreshToken, string clientId, string clientSecret);
    }
}
