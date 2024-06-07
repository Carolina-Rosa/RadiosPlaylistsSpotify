using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SpotifyPlaylistRadio.Models;

namespace SpotifyPlaylistRadio.Services
{
    public class PlaylistService
    {
        private readonly IMongoCollection<Playlist> _playlistCollection;
        private readonly ISpotifyService _spotifyService;
        private readonly RadiosService _radiosService;
        private readonly Playlists _playlists;
        private readonly AuthToken _authToken;

        public PlaylistService(
            IOptions<SpotifyPlaylistsFromRadioDatabaseSettings> spotifyPlaylistsFromRadioDatabaseSettings, ISpotifyService spotifyService, RadiosService radiosService, Playlists playlists, AuthToken authToken)

        {
            var mongoClient = new MongoClient(
                spotifyPlaylistsFromRadioDatabaseSettings.Value.ConnectionURI);

            var mongoDatabase = mongoClient.GetDatabase(
                spotifyPlaylistsFromRadioDatabaseSettings.Value.DatabaseName);

            _playlistCollection = mongoDatabase.GetCollection<Playlist>("Playlist");
            _spotifyService = spotifyService;
            _radiosService = radiosService;
            _playlists = playlists;
            _authToken = authToken;
        }

        public async Task<List<Playlist>> GetAsync() =>
            await _playlistCollection.Find(_ => true).ToListAsync();

        public async Task<Playlist?> GetAsync(string id) =>
            await _playlistCollection.Find(x => x.ID == id).FirstOrDefaultAsync();
        
        public async Task<Playlist> GetPlaylistInfoFromRadioName(string radioName)
        {

            var radio = await _radiosService.GetByNameAsync(radioName);
            if (radio == null)
            {
                return null;
            }

            var playlistName = "Listening " + radio.displayName;
            Playlist playlist = _playlists.items.FirstOrDefault((item) => item.Name == playlistName);

            if (playlist == null)
            {
                return null;
            }

            Playlist radioPlaylist = await _spotifyService.GetPlaylist(_authToken.access_token, playlist.ID);

            if (radioPlaylist != null)
            {
                radioPlaylist.TracksTotal = radioPlaylist.Tracks.total;
            }
            return radioPlaylist;
        }

        public async Task CreateAsync(Playlist newPlaylist) =>
            await _playlistCollection.InsertOneAsync(newPlaylist);

        public async Task UpdateAsync(string id, Playlist updatedPlaylist) =>
            await _playlistCollection.ReplaceOneAsync(x => x.ID == id, updatedPlaylist);

        public async Task RemoveAsync(string id) =>
            await _playlistCollection.DeleteOneAsync(x => x.ID == id);

    }

}

