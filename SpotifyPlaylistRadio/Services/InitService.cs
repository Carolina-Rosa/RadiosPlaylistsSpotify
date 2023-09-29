using Newtonsoft.Json;
using OpenQA.Selenium.Internal;
using SpotifyPlaylistRadio.Models;
using SpotifyPlaylistRadio.Models.ReadDataFromRadio;
using SpotifyPlaylistRadio.Socket;
using System.Net.WebSockets;

namespace SpotifyPlaylistRadio.Services
{
    public class InitService : IInitService
    {
        private readonly ISpotifyAccountService _spotifyAccountService;
        private readonly ISpotifyService _spotifyService;
        private readonly IConfiguration _configuration;
        private readonly ISocketMessages _socketMessages;
        private readonly RadiosService _radiosService;
        private readonly PlaylistService _playlistService;
        private readonly MusicSpotifyService _musicSpotifyService;
        private readonly ArtistService _artistsService;

        public InitService(ISpotifyAccountService spotifyAccountService, ISpotifyService spotifyService, IConfiguration configuration, ISocketMessages socketMessages, RadiosService radiosService, PlaylistService playlistService, MusicSpotifyService musicSpotifyService, ArtistService artistsService)
        {
            _spotifyAccountService = spotifyAccountService;
            _spotifyService = spotifyService;
            _configuration = configuration;
            _socketMessages = socketMessages;
            _radiosService = radiosService;
            _playlistService = playlistService;
            _musicSpotifyService = musicSpotifyService;
            _artistsService = artistsService;
        }

        public async Task something(HttpContext context, WebSocket ws)
        {
            int MAX_MAX_SIZE = 9000;
            int MIN_SIZE = 10;

            int maxSize =
                _configuration.GetSection("Playlist:MaxSize").Get<int>() > MAX_MAX_SIZE ||
                _configuration.GetSection("Playlist:MaxSize").Get<int>() < MIN_SIZE ?
                    _configuration.GetSection("Playlist:DefaultSize").Get<int>() :
                    _configuration.GetSection("Playlist:MaxSize").Get<int>();

            var lastMusicPlayed = "";

            CancellationToken ct = context.RequestAborted;
            string currentSubscriberId;

            currentSubscriberId = _socketMessages.AddSubscriber(ws);

            await _socketMessages.SendNotification(JsonConvert.SerializeObject(new SendSocketMessage { Message = "Connected", TimeStamp = DateTime.Now, MessageType = MessageType.Log, RadioName = "None" }));

            AuthToken authToken = await _spotifyAccountService.RefreshToken(_configuration["Spotify:RefreshToken"], _configuration["Spotify:ClientId"], _configuration["Spotify:ClientSecret"]);

            List<Radio> radiosList = _configuration.GetSection("Radios").Get<List<Radio>>();

            DateTime lastRefresh = DateTime.Now;

            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }

                await _socketMessages.SendNotification(JsonConvert.SerializeObject(new SendSocketMessage { Message = DateTime.Now.ToString(), TimeStamp = DateTime.Now, MessageType = MessageType.Log, RadioName = "None" }));

                foreach (Radio r in radiosList)
                {

                    //await _radiosService.CreateAsync(r);
                    SongScraped scrapedSong = await GetSongFromSite(r);
                    if (scrapedSong != null)
                    {

                        await _socketMessages.SendNotification(JsonConvert.SerializeObject(new SendSocketMessage { Message = "{\"Music\":\"" + scrapedSong.Title + "\", \"Artist\":\"" + scrapedSong.Artist + "\"}", TimeStamp = DateTime.Now, MessageType = MessageType.PlayingNow, RadioName = r.name }));
                        await _socketMessages.SendNotification(JsonConvert.SerializeObject(new SendSocketMessage { Message = "Music " + scrapedSong.Title + " is playing on " + r.name, TimeStamp = DateTime.Now, MessageType = MessageType.Log, RadioName = r.name }));

                        Playlist p = await GetPlaylist(authToken.access_token, _spotifyService, r.name);
                        if (p != null)
                        {

                            if (lastRefresh.Add(new TimeSpan(0, 50, 0)).CompareTo(DateTime.Now) < 0)
                            {
                                authToken = await _spotifyAccountService.RefreshToken(_configuration["Spotify:RefreshToken"], _configuration["Spotify:ClientId"], _configuration["Spotify:ClientSecret"]);
                                lastRefresh = DateTime.Now;
                            }

                            //SongScraped scrapedSong = new() { Title = "Tightrope", Artist = "Janelle Monáe c/ Big Boi" };

                            if (scrapedSong.Title != lastMusicPlayed)
                            {
                                lastMusicPlayed = scrapedSong.Title;

                                if (scrapedSong.Title != "" && scrapedSong.Artist != "" && !r.Podcasts.Contains(scrapedSong.Title))
                                {
                                    MusicSpotify music = await _spotifyService.SearchMusicPlaying(authToken.access_token, scrapedSong, r.name);

                                    if (music != null)
                                    {
                                        music.radioName = r.name;
                                        music.timestamp = DateTime.UtcNow;
                                        await _musicSpotifyService.CreateAsync(music);
                                        foreach(Artist a in music.artists)
                                        {
                                            a.radioName = r.name;
                                            a.timestamp = DateTime.UtcNow;
                                            await _artistsService.CreateAsync(a);

                                        }
                                        await _spotifyService.AddToPlaylist(authToken.access_token, p.id, music, r.name);
                                    }
                                }
                                else
                                {
                                    await _socketMessages.SendNotification(JsonConvert.SerializeObject(new SendSocketMessage { Message = "Playing podcast or info on " + r.name, TimeStamp = DateTime.Now, MessageType = MessageType.Log, RadioName = r.name }));
                                }
                            }

                            Playlist radioPlaylist = await _spotifyService.GetPlaylist(authToken.access_token, p.id);

                            if (radioPlaylist.Tracks.total > maxSize)
                            {
                                await _spotifyService.RemoveTracksWhenPlaylistReachesMaxSize(authToken.access_token, radioPlaylist, maxSize, r.name);
                            }
                        }
                    }
                }

                int SECONDS_TIL_REFRESH = 90;
                for (int i = 0; i < SECONDS_TIL_REFRESH; i++)
                {
                    await Task.Delay(1000);
                    await _socketMessages.SendNotification(JsonConvert.SerializeObject(new SendSocketMessage { Message = (SECONDS_TIL_REFRESH - i).ToString(), TimeStamp = DateTime.Now, MessageType = MessageType.Timer, RadioName = "None" }));
                }
            }

            if (!string.IsNullOrWhiteSpace(currentSubscriberId))
            {
                _socketMessages.RemoveSubscriber(currentSubscriberId);
                if (ws != null)
                {
                    await ws.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    ws.Dispose();
                }
            }

            return;
        }

        private async Task<Playlist> GetPlaylist(string access_token, ISpotifyService _spotifyService, string selectedRadio)
        {
            Playlists playlists = await _spotifyService.GetUsersPlaylist(access_token);
            

            if (playlists != null)
            {
                Playlist p = playlists.items.FirstOrDefault((item) => item.name == "Listening " + selectedRadio);


                p ??= await _spotifyService.CreatePlaylist(access_token, new Dictionary<string, string> {
                                        { "name", "Listening "+ selectedRadio},
                                        { "description", "Automatically updated playlist from what is playing in "+selectedRadio },
                                        { "public", "true" }
                                    });
                //await _playlistService.CreateAsync(p);

                return p;
            }

            return null;
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

