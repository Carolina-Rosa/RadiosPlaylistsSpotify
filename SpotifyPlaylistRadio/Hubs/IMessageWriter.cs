using SpotifyPlaylistRadio.Models;

namespace SpotifyPlaylistRadio.Hubs
{
    public interface IMessageWriter
    {
        Task Write(string message);
        Task SendMessageSocket(string message, MessageType messageType, string radioName);
    }
}
