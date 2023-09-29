using Microsoft.AspNetCore.Mvc;
using SpotifyPlaylistRadio.Models;
using SpotifyPlaylistRadio.Services;

namespace SpotifyPlaylistRadio.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RadiosController : ControllerBase
    {
        private readonly RadiosService _radiosService;

        public RadiosController(RadiosService radiosService) =>
            _radiosService = radiosService;

        [HttpGet]
        public async Task<List<Radio>> GetRadios() =>
            await _radiosService.GetAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Radio>> GetRadio(string id)
        {
            var radio = await _radiosService.GetAsync(id);

            if (radio is null)
            {
                return NotFound();
            }

            return radio;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRadio(Radio newRadio)
        {
            await _radiosService.CreateAsync(newRadio);

            return CreatedAtAction(nameof(GetRadio), new { id = newRadio.id }, newRadio);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRadio(string id, Radio updatedRadio)
        {
            var radio = await _radiosService.GetAsync(id);

            if (radio is null)
            {
                return NotFound();
            }

            updatedRadio.id = radio.id;

            await _radiosService.UpdateAsync(id, updatedRadio);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRadio(string id)
        {
            var radio = await _radiosService.GetAsync(id);

            if (radio is null)
            {
                return NotFound();
            }

            await _radiosService.RemoveAsync(id);

            return NoContent();
        }
    }
}
