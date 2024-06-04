using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SpotifyPlaylistRadio.Models;
using SpotifyPlaylistRadio.Models.Top5;

namespace SpotifyPlaylistRadio.Services
{
    public class MusicSpotifyService
    {
        private readonly IMongoCollection<MusicSpotify> _musicSpotifyCollection;
        private readonly ISpotifyService _spotifyService;
        private readonly AuthToken _authToken;

        public MusicSpotifyService(
            IOptions<SpotifyPlaylistsFromRadioDatabaseSettings> spotifyPlaylistsFromRadioDatabaseSettings, ISpotifyService spotifyService, AuthToken authToken)
        {
            var mongoClient = new MongoClient(
                spotifyPlaylistsFromRadioDatabaseSettings.Value.ConnectionURI);

            var mongoDatabase = mongoClient.GetDatabase(
                spotifyPlaylistsFromRadioDatabaseSettings.Value.DatabaseName);

            _musicSpotifyCollection = mongoDatabase.GetCollection<MusicSpotify>("MusicSpotify");
            _spotifyService = spotifyService;
            _authToken = authToken;
        }


        public async Task<List<MusicSpotify>> GetAsync() =>
            await _musicSpotifyCollection.Find(_ => true).ToListAsync();


        public async Task<MusicSpotify?> GetAsync(string id) =>
            await _musicSpotifyCollection.Find(x => x._id == id).FirstOrDefaultAsync();


        public async Task<List<TopSong>> GetTop5SongsAsync(TimeRange timeRange, string radio)
        {
            const string TOP5_SONGS_GLOBAL = "GLOBAL";

            DateTime timeForComparison = getDateTimeRange(timeRange);

            var allSongs = radio == TOP5_SONGS_GLOBAL ? await _musicSpotifyCollection.Find(x => x.timestamp > timeForComparison).ToListAsync() : await _musicSpotifyCollection.Find(x => x.radioName == radio && x.timestamp > timeForComparison).ToListAsync();

            List<TopSong> songsTimesPlayed = new List<TopSong>();

            foreach (var song in allSongs)
            {
                TopSong tS = songsTimesPlayed.Find(s => s.SongName == song.name);
                if (tS == null)
                    songsTimesPlayed.Add(new TopSong() { TimesPlayed = 1, SongName = song.name, ArtistName = song.artists.Select(i => i.name).Aggregate((i, j) => i + ", " + j) });
                else
                    tS.TimesPlayed += 1;
            }

            var topValues = songsTimesPlayed.OrderByDescending(x => x.TimesPlayed)
                             .Take(5).ToList();

            return topValues;
        }
        
        public async Task<MusicSpotify> GetWhatWasPlaying(string radioName, DateTime dateTime)
        {
            var allSongs = await _musicSpotifyCollection.Find(x => x.radioName == radioName && x.timestamp.CompareTo(dateTime.AddMinutes(-5))>0 && x.timestamp.CompareTo(dateTime.AddMinutes(1)) < 0).ToListAsync();

            if (allSongs.Count == 0)
            {
                return null;
            }

            var musicOnTime = new MusicSpotify();
            var timeDif = new TimeSpan(1,0,0);
            foreach(var song in allSongs)
            {
                if (dateTime.Subtract (song.timestamp) < timeDif)
                {
                    timeDif= dateTime.Subtract (song.timestamp);
                    musicOnTime = song;
                }
            }

            var musicPlayed = await _spotifyService.GetMusicByID(_authToken.access_token, musicOnTime.id);

            return musicPlayed;
        }

        public async Task CreateAsync(MusicSpotify newPlaylist) =>
            await _musicSpotifyCollection.InsertOneAsync(newPlaylist);


        public async Task UpdateAsync(string id, MusicSpotify updatedMusicSpotify) =>
            await _musicSpotifyCollection.ReplaceOneAsync(x => x._id == id, updatedMusicSpotify);


        public async Task RemoveAsync(string id) =>
            await _musicSpotifyCollection.DeleteOneAsync(x => x._id == id);


        private DateTime getDateTimeRange(TimeRange timeRange)
        {
            switch (timeRange)
            {
                case TimeRange.Last24Hours: 
                    return DateTime.UtcNow.AddDays(-1);
                case TimeRange.Last7Days: 
                    return DateTime.UtcNow.AddDays(-7);
                case TimeRange.Last30Days: 
                    return DateTime.UtcNow.AddDays(-30);
                case TimeRange.FromStart: 
                    return DateTime.UtcNow.AddYears(-20);
                default: 
                    return DateTime.UtcNow;
            }
        }
    }
}
