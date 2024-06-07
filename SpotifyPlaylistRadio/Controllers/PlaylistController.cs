using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotifyPlaylistRadio.Models;
using SpotifyPlaylistRadio.Services;

namespace SpotifyPlaylistRadio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly PlaylistService _playlistService;

        public PlaylistController(PlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        [HttpGet]
        public async Task<List<Playlist>> GetPlaylists()
        {
            List<Playlist> p = await _playlistService.GetAsync();
            return p;
        }
              
        [HttpGet("{radioName}")]
        public async Task<Playlist> GetPlaylistFromRadioName(string radioName)
        {
            var playlistInfo = await _playlistService.GetPlaylistInfoFromRadioName(radioName);

            return playlistInfo;
        }

    }
}
