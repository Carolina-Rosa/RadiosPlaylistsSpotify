using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SpotifyPlaylistRadio.Models;
using SpotifyPlaylistRadio.Models.Top5;
using System.Collections.Generic;

namespace SpotifyPlaylistRadio.Services
{
    public class ArtistService
    {
        private readonly IMongoCollection<Artist> _artistCollection;

        public ArtistService(
            IOptions<SpotifyPlaylistsFromRadioDatabaseSettings> spotifyPlaylistsFromRadioDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                spotifyPlaylistsFromRadioDatabaseSettings.Value.ConnectionURI);

            var mongoDatabase = mongoClient.GetDatabase(
                spotifyPlaylistsFromRadioDatabaseSettings.Value.DatabaseName);

            _artistCollection = mongoDatabase.GetCollection<Artist>("Artist");
        }

        public async Task<List<Artist>> GetAsync() =>
            await _artistCollection.Find(_ => true).ToListAsync();

        public async Task<Artist?> GetAsync(string id) =>
            await _artistCollection.Find(x => x._id == id).FirstOrDefaultAsync();


        public async Task<List<TopArtist>> GetTop5ArtistsAsync(string radio)
        {
            var allArtists = radio == "ALL" ? await _artistCollection.Find(_ => true).ToListAsync() : await _artistCollection.Find(x => x.radioName == radio).ToListAsync();

            List<TopArtist> artistTimesPlayed = new List<TopArtist>();

            foreach (var artist in allArtists)
            {
                TopArtist tA = artistTimesPlayed.Find(a => a.ArtistName == artist.name);
                if (tA == null)
                    artistTimesPlayed.Add(new TopArtist() { TimesPlayed = 1, ArtistName = artist.name });
                else
                    tA.TimesPlayed += 1;
            }

            var topValues = artistTimesPlayed.OrderByDescending(x => x.TimesPlayed)
                             .Take(5).ToList();

            return topValues;
        }


        public async Task CreateAsync(Artist newArtist) =>
            await _artistCollection.InsertOneAsync(newArtist);

        public async Task UpdateAsync(string id, Artist updatedArtist) =>
            await _artistCollection.ReplaceOneAsync(x => x._id == id, updatedArtist);

        public async Task RemoveAsync(string id) =>
            await _artistCollection.DeleteOneAsync(x => x._id == id);

    }
}
