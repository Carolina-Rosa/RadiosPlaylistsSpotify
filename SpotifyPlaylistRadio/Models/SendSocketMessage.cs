namespace SpotifyPlaylistRadio.Models
{
    public class SendSocketMessage
    {
        public string Message;
        public DateTime TimeStamp;
        public MessageType MessageType;
        public string RadioName;
    }

    public enum MessageType
    {
       PlayingNow,
       Log,
       Timer
    }
}
 