using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace SpotifyPlaylistRadio.Socket
{
    public class SocketMessage : ISocketMessages
    {
        private Dictionary<string, WebSocket> _subscribers = new Dictionary<string, WebSocket>();

        public string AddSubscriber(WebSocket subscriber)
        {
            var subscriberId = Guid.NewGuid().ToString();
            _subscribers.TryAdd(subscriberId, subscriber);
            return subscriberId.ToString();
        }

        public void RemoveSubscriber(string subscriberId)
        {
            WebSocket empty;
            _subscribers.Remove(subscriberId, out empty);
        }

        public WebSocket GetConnection(string id)
        {
            _subscribers.TryGetValue(id, out WebSocket socket);
            return socket;
        }

        public Dictionary<string, WebSocket> GetAllConnections()
        {
            return _subscribers;
        }

        public async Task SendNotification(string message)
        {
            foreach (var sub in _subscribers)
            {
                if (sub.Value.State == WebSocketState.Open)
                {
                    await SendMessage(sub.Value, message);
                }
            }
        }

        private async Task SendMessage(WebSocket socket, string data)
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
