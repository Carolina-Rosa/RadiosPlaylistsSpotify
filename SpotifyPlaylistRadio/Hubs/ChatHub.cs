using Microsoft.AspNetCore.SignalR;

namespace SpotifyPlaylistRadio.Hubs;

public sealed class ChatHub : Hub < IChatHub >
{
    //public override async Task OnConnectedAsync()
    //{
    //    await Clients.All.ReceiveMessage($"{Context.ConnectionId} has joined");
    //    // postman - {"protocol":"json","version":1}
    //}

    //public async Task SendMessage(string message)
    //{
    //    await Clients.All.ReceiveMessage($"{Context.ConnectionId}: {message}");
    //    //postman - {"arguments":["Test message"],"invocationId":"0","target":"SendMessage","type":1}
    //}
}
