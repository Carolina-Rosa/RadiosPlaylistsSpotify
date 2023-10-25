using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SpotifyPlaylistRadio.Models;

namespace SpotifyPlaylistRadio.Hubs
{
    public class MessageWriter : IMessageWriter
    {
        private readonly IHubContext<ChatHub, IChatHub> _hubContext;

        public MessageWriter(IHubContext<ChatHub, IChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Write(string message)
        {
            await _hubContext.Clients.All.ReceiveMessage(message);
        }

        public async Task SendMessageSocket(string message, MessageType messageType, string radioName)
        {
            await Write(JsonConvert.SerializeObject(new SendSocketMessage { Message = message, TimeStamp = DateTime.Now, MessageType = messageType, RadioName = radioName }));
            if (messageType!= MessageType.Timer)
                await Task.Delay(500);

        }
    }
}
