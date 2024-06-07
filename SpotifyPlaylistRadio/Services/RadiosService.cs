using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SpotifyPlaylistRadio.Models;

namespace SpotifyPlaylistRadio.Services
{
    public class RadiosService
    {
        private readonly IMongoCollection<Radio> _radiosCollection;

        public RadiosService(
            IOptions<SpotifyPlaylistsFromRadioDatabaseSettings> spotifyPlaylistsFromRadioDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                spotifyPlaylistsFromRadioDatabaseSettings.Value.ConnectionURI);

            var mongoDatabase = mongoClient.GetDatabase(
                spotifyPlaylistsFromRadioDatabaseSettings.Value.DatabaseName);

            _radiosCollection = mongoDatabase.GetCollection<Radio>("Radios");
        }

        public async Task<List<Radio>> GetAsync() =>
            await _radiosCollection.Find(_ => true).ToListAsync();

        public async Task<Radio?> GetAsync(string id) =>
            await _radiosCollection.Find(x => x.id == id).FirstOrDefaultAsync();
        
        public async Task<Radio?> GetByNameAsync(string radioName) =>
            await _radiosCollection.Find(x => x.name == radioName).FirstOrDefaultAsync();

        public async Task CreateAsync(Radio newRadio) =>
            await _radiosCollection.InsertOneAsync(newRadio);

        public async Task UpdateAsync(string id, Radio updatedRadio) =>
            await _radiosCollection.ReplaceOneAsync(x => x.id == id, updatedRadio);

        public async Task RemoveAsync(string id) =>
            await _radiosCollection.DeleteOneAsync(x => x.id == id);
    }
}
