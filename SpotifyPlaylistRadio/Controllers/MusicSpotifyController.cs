using Microsoft.AspNetCore.Mvc;
using SpotifyPlaylistRadio.Models;
using SpotifyPlaylistRadio.Models.Top5;
using SpotifyPlaylistRadio.Services;

namespace SpotifyPlaylistRadio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicSpotifyController : ControllerBase
    {
        private readonly MusicSpotifyService _musicSpotifyService;
        private readonly RadiosService _radiosService;

        public MusicSpotifyController(MusicSpotifyService musicSpotifyService, RadiosService radiosService)
        {
            _musicSpotifyService = musicSpotifyService;
            _radiosService = radiosService;
        }

        [HttpGet]
        public async Task<List<MusicSpotify>> GetMusicsSpotify()
        {
            List<MusicSpotify>  t = await _musicSpotifyService.GetAsync();
            return t;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MusicSpotify>> GetMusicSpotify(string id)
        {
            var musicSpotify = await _musicSpotifyService.GetAsync(id);

            if (musicSpotify is null)
            {
                return NotFound();
            }

            return musicSpotify;
        }
        
        [HttpGet("top5/{timeRange}")]
        public async Task<List<TopSongsByRadio>> GetTop5SongsByTimeRange(TimeRange timeRange = TimeRange.FromStart)
        {
            const string TOP5_SONGS_GLOBAL = "GLOBAL";

            var radios = await _radiosService.GetAsync();

            var allTop5Songs = new List<TopSongsByRadio>();

            var top5Songs = await _musicSpotifyService.GetTop5SongsAsync(timeRange, TOP5_SONGS_GLOBAL);
            allTop5Songs.Add(new TopSongsByRadio
            {
                TopType = "Global",
                TopSongs = top5Songs
            });

            foreach (var radio in radios)
            {
                top5Songs = await _musicSpotifyService.GetTop5SongsAsync(timeRange, radio.name);
                allTop5Songs.Add(new TopSongsByRadio
                {
                    TopType = radio.displayName,
                    TopSongs = top5Songs
                });
            }

            return allTop5Songs;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMusicSpotify(MusicSpotify newMusicSpotify)
        {
            await _musicSpotifyService.CreateAsync(newMusicSpotify);

            return CreatedAtAction(nameof(GetMusicSpotify), new { id = newMusicSpotify.id }, newMusicSpotify);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMusicSpotify(string id, MusicSpotify updatedMusicSpotify)
        {
            var musicSpotify = await _musicSpotifyService.GetAsync(id);

            if (musicSpotify is null)
            {
                return NotFound();
            }

            updatedMusicSpotify.id = musicSpotify.id;

            await _musicSpotifyService.UpdateAsync(id, updatedMusicSpotify);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMusicSpotify(string id)
        {
            var musicSpotify = await _musicSpotifyService.GetAsync(id);

            if (musicSpotify is null)
            {
                return NotFound();
            }

            await _musicSpotifyService.RemoveAsync(id);

            return NoContent();
        }
    }
}
