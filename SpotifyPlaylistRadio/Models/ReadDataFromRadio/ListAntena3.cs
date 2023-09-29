using Newtonsoft.Json;

namespace SpotifyPlaylistRadio.Models.ReadDataFromRadio
{
    public class ListAntena3: IDataFromRadio
    {
        public SongScraped ReadThisData(string data, string scrapingSongTitle, string scrapingSongArtist)
        {
            List<Dictionary<string, string>> dataToList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(data);

            SongScraped song = new()
            {
                Title = dataToList[0][scrapingSongTitle],

                Artist = dataToList[0][scrapingSongArtist]
            };

            Console.WriteLine(song.Title);

            return song;
        }
    }
}
