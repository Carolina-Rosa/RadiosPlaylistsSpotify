namespace SpotifyPlaylistRadio.Models
{
    public class MusicPlayed
    {
        public string name { get; set; }
        public List<Artist> artists { get; set; }
        public ExternalURLs external_urls { get; set; }
        public Album album { get; set; }
       
    }
}