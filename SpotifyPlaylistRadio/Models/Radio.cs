using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SpotifyPlaylistRadio.Models
{
    public class Radio
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id;
        public string name { get; set; }
        public string displayName { get; set; }
        public string dataFormat { get; set; }
        public string scrapingSongTitle { get; set; }
        public string scrapingSongArtist { get; set; }
        public string url { get; set; }
        public List<string> Podcasts { get; set; }
    }
}
