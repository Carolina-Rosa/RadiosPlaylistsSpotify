using Microsoft.AspNetCore.WebUtilities;
using SpotifyPlaylistRadio.Models;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using SpotifyPlaylistRadio.Socket;

namespace SpotifyPlaylistRadio.Services
{
    public class SpotifyService : ISpotifyService
    {
        private readonly HttpClient _httpClient;
        private readonly ISearchHelperService _searchHelperService;
        private readonly ISocketMessages _socketMessages;

        public SpotifyService(HttpClient httpClient, ISearchHelperService searchHelperService, ISocketMessages socketMessages)
        {
            _httpClient = httpClient;
            _searchHelperService = searchHelperService;
            _socketMessages = socketMessages;
        }

        public async Task<Playlists> GetUsersPlaylist(string token)
        {
            string user_id = await GetCurrentUser(token);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync(
                "users/" + user_id + "/playlists");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Playlists>(jsonString);
            }

            return null;
        }

        public async Task<string> GetCurrentUser(string token)
        {
            User user = null;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("me");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<User>(jsonString);
            }

            return user.id;
        }

        public async Task<Playlist> CreatePlaylist(string token, Dictionary<string, string> playlistInfo)
        {
            string user_id = await GetCurrentUser(token);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpContent content = new StringContent(JsonConvert.SerializeObject(playlistInfo), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(
                "https://api.spotify.com/v1/users/" + user_id + "/playlists", content);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                //TODO - Send log  - playlist x was creeated

                return JsonConvert.DeserializeObject<Playlist>(jsonString);
            }
            return null;
        }
        
        public async Task<Playlist> GetPlaylist(string token, string playlistId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync(
                "https://api.spotify.com/v1/playlists/" + playlistId);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Playlist>(jsonString);
            }
            return null;
        }
        
        public async Task RemoveTracksWhenPlaylistReachesMaxSize(string token, Playlist playlist, int playlistMaxSize, string radioName)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            int limit = playlist.Tracks.total - playlistMaxSize;

            HttpResponseMessage response = await _httpClient.GetAsync(
                "https://api.spotify.com/v1/playlists/" + playlist.id + "/tracks?limit=" + limit + "&offset=" + playlistMaxSize);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var playlistOfItemsToRemove = JsonConvert.DeserializeObject<TracksFromPlaylist>(jsonString);
                foreach (TrackFromPlaylist t in playlistOfItemsToRemove.items)
                {
                    await DeleteSongFromPlaylist(token, playlist.id, t.track.uri);

                    await _socketMessages.SendNotification(JsonConvert.SerializeObject(new SendSocketMessage { Message = "Playlist " + playlist.name + " reached Max Size. Remove song " + t.track.name, TimeStamp = DateTime.Now, MessageType = MessageType.Log, RadioName = radioName }));


                    //TODO - Send log  - playlist X reached Max Size remove song y

                }
            }
        }

        public async Task<MusicSpotify> SearchMusicPlaying(string token, SongScraped song, string radioName)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new Dictionary<string, string>
            {
                { "query", _searchHelperService.PrepareSongTitleForSearch(song.Title)},
                { "type", "track" },
                { "limit","50" }
            };

            HttpResponseMessage response = await _httpClient.GetAsync(QueryHelpers.AddQueryString("https://api.spotify.com/v1/search", content));

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                Search s = JsonConvert.DeserializeObject<Search>(jsonString);

                MusicSpotify searchResult = await _searchHelperService.CompareSongScrapedWithSearchResults(song.Artist, s.tracks.items);
                //TODO - Send log  - found music being searched or music not found 
                if (searchResult != null)
                {
                    await _socketMessages.SendNotification(JsonConvert.SerializeObject(new SendSocketMessage { Message = "Music found: " + song.Title, TimeStamp = DateTime.Now, MessageType = MessageType.Log, RadioName = radioName }));

                }
                else
                {
                    await _socketMessages.SendNotification(JsonConvert.SerializeObject(new SendSocketMessage { Message = "Music not found: " + song.Title, TimeStamp = DateTime.Now, MessageType = MessageType.Log, RadioName = radioName }));
                    await _socketMessages.SendNotification(JsonConvert.SerializeObject(new SendSocketMessage { Message = "Info searching - Not found: Title - " + song.Title + " // Artist - " + song.Artist, TimeStamp = DateTime.Now, MessageType = MessageType.Log, RadioName = radioName }));

                }
                
                return searchResult;
            }
            return null;
        }

        public async Task<Playlist> AddToPlaylist(string token, string playlist_id, MusicSpotify music, string radioName)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            HttpContent content = new StringContent(JsonConvert.SerializeObject(new MusicToAddPlaylist {
                uris = new List<string> { music.uri },
                position = 0
            }), Encoding.UTF8, "application/json") ;

            await DeleteSongFromPlaylist(token, playlist_id, music.uri);

            HttpResponseMessage response = await _httpClient.PostAsync(
                "https://api.spotify.com/v1/playlists/" + playlist_id + "/tracks", content);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                await _socketMessages.SendNotification(JsonConvert.SerializeObject(new SendSocketMessage { Message = "Music " + music.name + " was added to the playlist " + playlist_id, TimeStamp = DateTime.Now, MessageType = MessageType.Log, RadioName = radioName }));

                //TODO - Send log  - add music X to playlist Y
                return JsonConvert.DeserializeObject<Playlist>(jsonString);
            }

            return null;
        }
        
        public async Task DeleteSongFromPlaylist(string token, string playlist_id, string music)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpRequestMessage requestDelete = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri("https://api.spotify.com/v1/playlists/" + playlist_id + "/tracks"),
                    Content = new StringContent(JsonConvert.SerializeObject(new Dictionary<string, List<Dictionary<string, string>>> 
                    {
                        { "tracks", new List<Dictionary<string, string>> {
                            new Dictionary<string, string>
                            {
                                { "uri", music }
                            }
                        }}
                    }), Encoding.UTF8, "application/json")
                };

            var responseDelete = await _httpClient.SendAsync(requestDelete);

            if (responseDelete.IsSuccessStatusCode)
            {
                //TODO - Send log  - delete music X from playlist Y

                Console.WriteLine("It Deleted the Song");
            }
        } 
    }
}
