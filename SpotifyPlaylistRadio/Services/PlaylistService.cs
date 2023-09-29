using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SpotifyPlaylistRadio.Models;

namespace SpotifyPlaylistRadio.Services
{
    public class PlaylistService
    {
        private readonly IMongoCollection<Playlist> _playlistCollection;

        public PlaylistService(
            IOptions<SpotifyPlaylistsFromRadioDatabaseSettings> spotifyPlaylistsFromRadioDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                spotifyPlaylistsFromRadioDatabaseSettings.Value.ConnectionURI);

            var mongoDatabase = mongoClient.GetDatabase(
                spotifyPlaylistsFromRadioDatabaseSettings.Value.DatabaseName);

            _playlistCollection = mongoDatabase.GetCollection<Playlist>("Playlist");
        }

        public async Task<List<Playlist>> GetAsync() =>
            await _playlistCollection.Find(_ => true).ToListAsync();

        public async Task<Playlist?> GetAsync(string id) =>
            await _playlistCollection.Find(x => x.id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Playlist newPlaylist) =>
            await _playlistCollection.InsertOneAsync(newPlaylist);

        public async Task UpdateAsync(string id, Playlist updatedPlaylist) =>
            await _playlistCollection.ReplaceOneAsync(x => x.id == id, updatedPlaylist);

        public async Task RemoveAsync(string id) =>
            await _playlistCollection.DeleteOneAsync(x => x.id == id);

    }

}

