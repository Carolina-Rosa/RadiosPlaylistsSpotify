using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using SpotifyPlaylistRadio.Models;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using static System.Net.WebRequestMethods;

namespace SpotifyPlaylistRadio
{

    public static class SpotifyAPIInteractor
    {
        static HttpClient client = new();
        public static async Task<String> UserAuthorization()
        {
            var response_type = "code";
            var redirect_uri = "https://localhost:7269/";
            var scope = "user-read-private user-read-email";
            var show_dialog = "true";

            var toAuthorize = new Dictionary<string, string>
            {
                { "response_type", response_type },
                { "redirect_uri", redirect_uri },
                { "show_dialog", show_dialog },
                { "scope", scope }
            };

            HttpResponseMessage authorizeResponse = await client.GetAsync(QueryHelpers.AddQueryString("https://accounts.spotify.com/authorize", toAuthorize));

            if (authorizeResponse.IsSuccessStatusCode)
            {
                string jsonString = await authorizeResponse.Content.ReadAsStringAsync();
                Console.WriteLine(jsonString);
                //authCode = JsonConvert.DeserializeObject<AuthorizeCode>(jsonString);
            }

            return "string";
        }
        

        //public static async Task<string> GetCurrentUser(Token token)
        //{
        //    User user = null;

        //    token = await RefreshToken(token);

        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.token_type, token.access_token);

        //    HttpResponseMessage response = await client.GetAsync(
        //        "https://api.spotify.com/v1/me");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        string jsonString = await response.Content.ReadAsStringAsync();
        //        user = JsonConvert.DeserializeObject<User>(jsonString);
        //    }

        //    return user.id;
        //}

        //public static async Task<Playlists> GetUsersPlaylist(Token token)
        //{
        //    string user_id = await GetCurrentUser(token);

        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.token_type, token.access_token);

        //    HttpResponseMessage response = await client.GetAsync(
        //        "https://api.spotify.com/v1/users/" + user_id + "/playlists");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        string jsonString = await response.Content.ReadAsStringAsync();
        //        return JsonConvert.DeserializeObject<Playlists>(jsonString);
        //    }

        //    return null;
        //}

        //public static async Task<Playlist> CreatePlaylist(Token token)
        //{
        //    string user_id = await GetCurrentUser(token);

        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.token_type, token.access_token);

        //    HttpContent content = new StringContent(JsonConvert.SerializeObject(new Dictionary<string, string> {
        //            { "name", "Listening Antena3" },
        //            { "description", "Automatically updated playlist from what is playing in Antena 3" },
        //            { "public", "true" }
        //        }), Encoding.UTF8, "application/json");

        //    HttpResponseMessage response = await client.PostAsync(
        //        "https://api.spotify.com/v1/users/" + user_id + "/playlists", content);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        string jsonString = await response.Content.ReadAsStringAsync();
        //        return JsonConvert.DeserializeObject<Playlist>(jsonString);
        //    }
        //    return null;
        //}

        //public static async Task<Music> SearchMusicPlaying(Token token, SongScraped song)
        //{
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.token_type, token.access_token);

        //    var toAuthorize = new Dictionary<string, string>
        //    {
        //        { "query", song.Title },
        //        { "type", "track" },
        //        { "limit","50" }
        //    };

        //    HttpResponseMessage response = await client.GetAsync(QueryHelpers.AddQueryString("https://api.spotify.com/v1/search", toAuthorize));

        //    if (response.IsSuccessStatusCode)
        //    {
        //        string jsonString = await response.Content.ReadAsStringAsync();
        //        Search s = JsonConvert.DeserializeObject<Search>(jsonString);

        //        foreach (Music item in s.tracks.items)
        //        {
        //            if (item.artists[0].name.ToLower() == song.Artist.ToLower())
        //            {
        //                return item;
        //            }
        //        }
        //    }
        //    return null;
        //}

        //public static async Task<Playlist> AddToPlaylist(Token token, string playlist_id, Music music)
        //{
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.token_type, token.access_token);

        //    HttpContent content = new StringContent(JsonConvert.SerializeObject(new Dictionary<string, List<string>> {
        //            { "uris", new List<string>(){music.uri} },
        //        }), Encoding.UTF8, "application/json");

        //    HttpResponseMessage response = await client.PostAsync(
        //        "https://api.spotify.com/v1/playlists/" + playlist_id + "/tracks", content);


        //    if (response.IsSuccessStatusCode)
        //    {
        //        string jsonString = await response.Content.ReadAsStringAsync();
        //        return JsonConvert.DeserializeObject<Playlist>(jsonString);

        //    }
        //    return null;
        //}
    }
}
