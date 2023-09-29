using System.Net.WebSockets;

namespace SpotifyPlaylistRadio.Services
{
    public interface IInitService
    {
        Task something(HttpContext context, WebSocket ws);
    }
}
