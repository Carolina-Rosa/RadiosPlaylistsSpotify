using SpotifyPlaylistRadio.Models;

namespace SpotifyPlaylistRadio.Services
{
    public interface ISearchHelperService
    {
        Task<MusicSpotify> CompareSongScrapedWithSearchResults(string songArtist, List<MusicSpotify> searchResults, string radioName);
        string PrepareSongTitleForSearch(string songTitle);
    }
}
