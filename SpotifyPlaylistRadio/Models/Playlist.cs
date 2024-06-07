using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SpotifyPlaylistRadio.Models

{
    public class Playlist
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string _id { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ExternalURLs external_urls { get; set; }
        public List<Image> images { get; set; }
        public Followers Followers { get; set; }
        public Radio Radio { get; set; }
        public Tracks Tracks { get; set; }
        public int TracksTotal { get; set; }

    }
}
