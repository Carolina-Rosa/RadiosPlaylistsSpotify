using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SpotifyPlaylistRadio.Models
{
    public class MusicSpotify
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string uri { get; set; }
        public List<Artist> artists { get; set; }
        public ExternalURLs external_urls { get; set; }
        public Album album { get; set; }
        public string radioName { get; set; }
        public DateTime timestamp { get; set; }

    }
}
