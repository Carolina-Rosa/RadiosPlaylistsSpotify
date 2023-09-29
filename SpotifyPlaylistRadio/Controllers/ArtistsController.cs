using Microsoft.AspNetCore.Mvc;
using SpotifyPlaylistRadio.Models;
using SpotifyPlaylistRadio.Models.Top5;
using SpotifyPlaylistRadio.Services;

namespace SpotifyPlaylistRadio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly ArtistService _artistService;
        private readonly RadiosService _radiosService;

        public ArtistsController(ArtistService artistService, RadiosService radiosService)
        {
            _artistService = artistService;
            _radiosService = radiosService;
        } 

        [HttpGet]
        public async Task<List<Artist>> GetArtists()
        {
            List<Artist> t = await _artistService.GetAsync();
            return t;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Artist>> GetArtist(string id)
        {
            var artist = await _artistService.GetAsync(id);

            if (artist is null)
            {
                return NotFound();
            }

            return artist;
        }
        
        [HttpGet("top5")]
        public async Task<List<TopArtistByRadio>> GetTop5Artist()
        {
            var radios = await _radiosService.GetAsync();

            var allTop5Artists = new List<TopArtistByRadio>();
           

            var top5Artists = await _artistService.GetTop5ArtistsAsync("ALL");
            allTop5Artists.Add(new TopArtistByRadio { 
                TopType = "Global",
                TopArtists = top5Artists });

            foreach ( var radio in radios)
            {
                top5Artists = await _artistService.GetTop5ArtistsAsync(radio.name);
                allTop5Artists.Add(new TopArtistByRadio
                {
                    TopType = radio.name,
                    TopArtists = top5Artists
                });
            }

            return allTop5Artists;
        }

        [HttpGet("top5/{radio}")]
        public async Task<List<TopArtist>> GetTop5Artist(string radio)
        {
            var top5Artists = await _artistService.GetTop5ArtistsAsync(radio);

            return top5Artists;
        }

        [HttpPost]
        public async Task<IActionResult> CreateArtist(Artist newArtist)
        {
            await _artistService.CreateAsync(newArtist);

            return CreatedAtAction(nameof(GetArtist), new { id = newArtist.id }, newArtist);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArtist(string id, Artist updatedArtist)
        {
            var artist = await _artistService.GetAsync(id);

            if (artist is null)
            {
                return NotFound();
            }

        updatedArtist.id = artist.id;

            await _artistService.UpdateAsync(id, updatedArtist);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtist(string id)
        {
            var artist = await _artistService.GetAsync(id);

            if (artist is null)
            {
                return NotFound();
            }

            await _artistService.RemoveAsync(id);

            return NoContent();
        }
    }
}