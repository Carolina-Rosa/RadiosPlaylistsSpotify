namespace SpotifyPlaylistRadio.Models.ReadDataFromRadio
{
    public interface IDataFromRadio
    {
        SongScraped ReadThisData(string data, string scrapingSongTitle, string scrapingSongArtist);
    }
}
