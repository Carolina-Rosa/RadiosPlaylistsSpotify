using System.Xml.Serialization;

namespace SpotifyPlaylistRadio.Models.ReadDataFromRadio
{
    public class RadioInfo : IDataFromRadio
    {
        public Table Table { get; set; }

        public SongScraped ReadThisData(string data, string scrapingSongTitle, string scrapingSongArtist)
        {
            XmlSerializer serializer = new(typeof(RadioInfo));
            StringReader rdr = new StringReader(data);
            RadioInfo config = (RadioInfo)serializer.Deserialize(rdr);

            SongScraped song = new()
            {
                Title = config.Table.DB_DALET_TITLE_NAME,
                Artist = config.Table.DB_DALET_ARTIST_NAME
            };

            Console.WriteLine(song.Title);

            return song;
        }

    }
}
