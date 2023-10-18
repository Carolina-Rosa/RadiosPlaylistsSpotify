namespace SpotifyPlaylistRadio.Hubs
{
    public interface IChatHub
    {
        Task ReceiveMessage(string message);
    }
}
