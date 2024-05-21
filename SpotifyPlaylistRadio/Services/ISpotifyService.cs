using SpotifyPlaylistRadio.Models;

namespace SpotifyPlaylistRadio.Services
{
    public interface ISpotifyService
    {
        Task<Playlists> GetUsersPlaylist(string token);
        Task<string> GetCurrentUser(string token);
        Task<Playlist> CreatePlaylist(string token, Dictionary<string, string> playlistInfo);
        Task<Playlist> GetPlaylist(string token, string playlistId);
        Task RemoveTracksWhenPlaylistReachesMaxSize(string token, Playlist playlist, int playlistMaxSize, string radioName);
        Task<MusicSpotify> SearchMusicPlaying(string token, SongScraped song, string radioName);
        Task<MusicPlayed> GetMusicByID(string token, string ID);
        Task<Playlist> AddToPlaylist(string token, string playlist_id, MusicSpotify music, string radioName);
    }
}
