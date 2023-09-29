using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SpotifyPlaylistRadio.Models

{
    public class Playlist
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string _id { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public Radio radio { get; set; }
        public Tracks Tracks { get; set; }

    }
}
