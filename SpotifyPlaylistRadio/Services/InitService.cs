using Newtonsoft.Json;
using SpotifyPlaylistRadio.Hubs;
using SpotifyPlaylistRadio.Models;
using SpotifyPlaylistRadio.Models.ReadDataFromRadio;

namespace SpotifyPlaylistRadio.Services
{
    public class InitService : IInitService
    {
        private readonly ISpotifyAccountService _spotifyAccountService;
        private readonly ISpotifyService _spotifyService;
        private readonly IConfiguration _configuration;
        private readonly RadiosService _radiosService;
        private readonly PlaylistService _playlistService;
        private readonly MusicSpotifyService _musicSpotifyService;
        private readonly ArtistService _artistsService;
        private readonly RadiosService _radioService;
        private readonly IMessageWriter _messageWriter;


        public InitService(ISpotifyAccountService spotifyAccountService, ISpotifyService spotifyService, IConfiguration configuration, RadiosService radiosService, PlaylistService playlistService, MusicSpotifyService musicSpotifyService, ArtistService artistsService, RadiosService radioService, IMessageWriter messageWriter)
        {
            _spotifyAccountService = spotifyAccountService;
            _spotifyService = spotifyService;
            _configuration = configuration;
            _radiosService = radiosService;
            _playlistService = playlistService;
            _musicSpotifyService = musicSpotifyService;
            _artistsService = artistsService;
            _radioService = radioService;
            _messageWriter = messageWriter;
        }

        public async Task something()
        {
            await _messageWriter.SendMessageSocket("Connected", MessageType.Log, "None");

            AuthToken authToken = await _spotifyAccountService.RefreshToken(_configuration["Spotify:RefreshToken"], _configuration["Spotify:ClientId"], _configuration["Spotify:ClientSecret"]);

            List<Radio> radiosList = await _radioService.GetAsync();

            DateTime lastRefresh = DateTime.Now;

            var lastMusicPlayedOnRadio = radiosList.ToDictionary(keySelector: radio => radio.name, elementSelector: music => "");
           
            Playlists playlists = null;
            
            while (playlists == null)
            {
                playlists = await _spotifyService.GetUsersPlaylist(authToken.access_token);
            }

            while (true)
            {
                foreach (Radio r in radiosList)
                {
                    SongScraped scrapedSong = await GetSongFromSite(r);
                    if (scrapedSong != null)
                    {
                        Playlist playlist = await GetPlaylist(playlists, authToken.access_token, r.displayName);
                        if (playlist != null)
                        {
                            if (NeedsNewRefresh(lastRefresh))
                            {
                                authToken = await NewRefreshToken();
                                lastRefresh = DateTime.Now;
                            }

                            await _messageWriter.SendMessageSocket("{\"Music\":\"" + scrapedSong.Title + "\", \"Artist\":\"" + scrapedSong.Artist + "\"}", MessageType.PlayingNow, r.name);
                            if (!IsLastMusicPlayedOnRadio(scrapedSong.Title, lastMusicPlayedOnRadio[r.name]))
                            {
                                lastMusicPlayedOnRadio[r.name] = scrapedSong.Title;

                                await _messageWriter.SendMessageSocket("{\"Music\":\"" + scrapedSong.Title + "\", \"Artist\":\"" + scrapedSong.Artist + "\"}", MessageType.PlayingNow, r.name);

                                if (IsPodcast(scrapedSong, r))
                                    await _messageWriter.SendMessageSocket("Playing podcast or info on " + r.displayName, MessageType.Log, r.name);
                                else
                                {
                                    await _messageWriter.SendMessageSocket("Music " + scrapedSong.Title + " is playing on " + r.displayName, MessageType.Log, r.name);

                                    await SearchAndAddSongToPlaylist(authToken.access_token, scrapedSong, r.name, playlist.id);

                                    await RemoveTrackIfReachesMax(authToken.access_token, r.name, playlist.id);
                                }

                            }
                            else
                                await _messageWriter.SendMessageSocket("Still playing " + scrapedSong.Title + " on " + r.displayName, MessageType.Log, r.name);
                        }
                    }
                    await Task.Delay(10);

                }
                await TimerToRefresh();
            }

            return;
        }

        private static bool NeedsNewRefresh(DateTime lastRefresh)
        {
            return lastRefresh.Add(new TimeSpan(0, 50, 0)).CompareTo(DateTime.Now) < 0;
        }

        private static bool IsPodcast(SongScraped scrapedSong, Radio r)
        {
            return !(scrapedSong.Title != "" && scrapedSong.Artist != "" && !r.Podcasts.Contains(scrapedSong.Title));
        }

        private static bool IsLastMusicPlayedOnRadio(string songTitle, string lastMusicPlayedOnRadio)
        {
            return songTitle == lastMusicPlayedOnRadio;
        }

        private async Task<AuthToken> NewRefreshToken()
        {
            return await _spotifyAccountService.RefreshToken(_configuration["Spotify:RefreshToken"], _configuration["Spotify:ClientId"], _configuration["Spotify:ClientSecret"]);
        }

        private async Task RemoveTrackIfReachesMax(string accessToken, string radioName, string playlistID)
        {
            int MAX_MAX_SIZE = 9000;
            int MIN_SIZE = 10;

            int maxSize =
                _configuration.GetSection("Playlist:MaxSize").Get<int>() > MAX_MAX_SIZE ||
                _configuration.GetSection("Playlist:MaxSize").Get<int>() < MIN_SIZE ?
                    _configuration.GetSection("Playlist:DefaultSize").Get<int>() :
                    _configuration.GetSection("Playlist:MaxSize").Get<int>();

            Playlist radioPlaylist = await _spotifyService.GetPlaylist(accessToken, playlistID);

            if (radioPlaylist.Tracks.total > maxSize)
            {
                await _spotifyService.RemoveTracksWhenPlaylistReachesMaxSize(accessToken, radioPlaylist, maxSize, radioName);
            }
        }

        private async Task SearchAndAddSongToPlaylist(string accessToken, SongScraped scrapedSong, string radioName, string playlistID)
        {
            MusicSpotify music = await _spotifyService.SearchMusicPlaying(accessToken, scrapedSong, radioName);

            if (music != null)
            {
                await _spotifyService.AddToPlaylist(accessToken, playlistID, music, radioName);

                await PrepareAndAddInfoToDB(music, radioName);
            }
        }


        private async Task PrepareAndAddInfoToDB(MusicSpotify music, string radioName)
        {
            music.radioName = radioName;
            music.timestamp = DateTime.UtcNow;
            await _musicSpotifyService.CreateAsync(music);
            foreach (Artist a in music.artists)
            {
                a.radioName = radioName;
                a.timestamp = DateTime.UtcNow;
                await _artistsService.CreateAsync(a);
            }
        }

        private async Task TimerToRefresh()
        {
            int SECONDS_TIL_REFRESH = 90;
            for (int i = 0; i < SECONDS_TIL_REFRESH; i++)
            {
                await Task.Delay(1000);
                await _messageWriter.SendMessageSocket((SECONDS_TIL_REFRESH - i).ToString(), MessageType.Timer, "None");
            }
        }

        private async Task<Playlist> GetPlaylist(Playlists playlists, string access_token, string selectedRadio)
        {
            Playlist p = playlists.items.FirstOrDefault((item) => item.name == "Listening " + selectedRadio);
            if (p == null)
            {
                p = await _spotifyService.CreatePlaylist(access_token, new Dictionary<string, string> {
                                        { "name", "Listening "+ selectedRadio},
                                        { "description", "Automatically updated playlist from what is playing in "+selectedRadio },
                                        { "public", "true" }
                                    });
                playlists.items.Add(p);

                await _playlistService.CreateAsync(p);
            }
            return p;
        }

        private static async Task<SongScraped> GetSongFromSite(Radio selectedRadio)
        {
            var data = await GetDataFromRadio(selectedRadio.url);
            ReadDataFactory factory = new ReadDataFactory();

            IDataFromRadio dataFromRadio = factory.readDataFromRadio(selectedRadio.dataFormat);

            return dataFromRadio.ReadThisData(data, selectedRadio.scrapingSongTitle, selectedRadio.scrapingSongArtist);
        }

        private static async Task<string> GetDataFromRadio(string url)
        {
            var client = new HttpClient();

            //TODO - try catch
            return await client.GetStringAsync(url);
        }
    }
}

