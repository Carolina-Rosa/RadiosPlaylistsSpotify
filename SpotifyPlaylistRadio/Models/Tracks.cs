using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SpotifyPlaylistRadio.Models
{
    public class Tracks
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id;
        public List<MusicSpotify> items;
        public int total;
    }
}
