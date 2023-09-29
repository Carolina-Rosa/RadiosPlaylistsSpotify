using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SpotifyPlaylistRadio.Models;
using SpotifyPlaylistRadio.Models.Top5;

namespace SpotifyPlaylistRadio.Services
{
    public class MusicSpotifyService
    {
        private readonly IMongoCollection<MusicSpotify> _musicSpotifyCollection;

        public MusicSpotifyService(
            IOptions<SpotifyPlaylistsFromRadioDatabaseSettings> spotifyPlaylistsFromRadioDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                spotifyPlaylistsFromRadioDatabaseSettings.Value.ConnectionURI);

            var mongoDatabase = mongoClient.GetDatabase(
                spotifyPlaylistsFromRadioDatabaseSettings.Value.DatabaseName);

            _musicSpotifyCollection = mongoDatabase.GetCollection<MusicSpotify>("MusicSpotify");
        }

        public async Task<List<MusicSpotify>> GetAsync() =>
            await _musicSpotifyCollection.Find(_ => true).ToListAsync();

        public async Task<MusicSpotify?> GetAsync(string id) =>
            await _musicSpotifyCollection.Find(x => x._id == id).FirstOrDefaultAsync();

        public async Task<List<TopSong>> GetTop5SongsAsync(string radio)
        {
            var allSongs = radio == "ALL" ? await _musicSpotifyCollection.Find(_ => true).ToListAsync() : await _musicSpotifyCollection.Find(x => x.radioName == radio).ToListAsync();

            List<TopSong> songsTimesPlayed = new List<TopSong>();

            foreach (var song in allSongs)
            {
                TopSong tA = songsTimesPlayed.Find(a => a.SongName == song.name);
                if (tA == null)
                    songsTimesPlayed.Add(new TopSong() { TimesPlayed = 1, SongName = song.name, ArtistName = song.artists.Select(i => i.name).Aggregate((i, j) => i + ", " + j) });
                else
                    tA.TimesPlayed += 1;
            }

            var topValues = songsTimesPlayed.OrderByDescending(x => x.TimesPlayed)
                             .Take(5).ToList();

            Console.WriteLine(topValues);

            return topValues;
        }


        public async Task CreateAsync(MusicSpotify newPlaylist) =>
            await _musicSpotifyCollection.InsertOneAsync(newPlaylist);

        public async Task UpdateAsync(string id, MusicSpotify updatedMusicSpotify) =>
            await _musicSpotifyCollection.ReplaceOneAsync(x => x._id == id, updatedMusicSpotify);

        public async Task RemoveAsync(string id) =>
            await _musicSpotifyCollection.DeleteOneAsync(x => x._id == id);

    }
}
