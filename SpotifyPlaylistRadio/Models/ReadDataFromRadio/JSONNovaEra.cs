using Newtonsoft.Json;

namespace SpotifyPlaylistRadio.Models.ReadDataFromRadio
{
    public class JSONNovaEra : IDataFromRadio
    {
        public SongScraped ReadThisData(string data, string scrapingSongTitle, string scrapingSongArtist)
        {
            Console.WriteLine(data);
            Dictionary<string, Object> dataToDict = JsonConvert.DeserializeObject<Dictionary<string, Object>>(data);
            
            if (Convert.ToInt32(dataToDict["status"]) == 4)
            {

                SongScraped song = new()
                {
                    Title = dataToDict[scrapingSongTitle]?.ToString(),

                    Artist = dataToDict[scrapingSongArtist]?.ToString()
                };
                Console.WriteLine(song.Title);

                return song;
            }
            return null;

        }
    }
}
