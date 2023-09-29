using System.Net.WebSockets;

namespace SpotifyPlaylistRadio.Socket
{
    public interface ISocketMessages
    {
        string AddSubscriber(WebSocket subscriber);
        void RemoveSubscriber(string subscriberId);
        Task SendNotification(string message);
    }
}