using System.Xml.Serialization;

namespace SpotifyPlaylistRadio.Models.ReadDataFromRadio
{
    [XmlRoot("music")]
    public class Music : IDataFromRadio
    {
        [XmlElement("song")]
        public Song song { get; set; }

        public SongScraped ReadThisData(string data, string scrapingSongTitle, string scrapingSongArtist)
        {
            XmlSerializer serializer = new(typeof(Music));
            StringReader reader = new StringReader(data);
            Music config = (Music)serializer.Deserialize(reader);

            SongScraped songScr = new()
            {
                Title = config.song.name,
                Artist = config.song.artist
            };

            Console.WriteLine(songScr.Title);

            return songScr;
        }

    }
}
