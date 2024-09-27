using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SuperHeldAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeldController : ControllerBase
    {
        private readonly DatenzugriffKontext _kontext;

        public SuperHeldController(DatenzugriffKontext kontext)
        {
            _kontext = kontext;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHeld>>> Get()
        {
            if (_kontext.SuperHelden == null)
                return NotFound("Keine SuperHelden gefunden.");
            return Ok(await _kontext.SuperHelden.ToListAsync());
        }

        [HttpGet("ID")]
        public async Task<ActionResult<SuperHeld>> Get(int ID) //Hier kann man List Paramater verwenden.
        {
            if (_kontext.SuperHelden == null)
                return NotFound("Keine SuperHelden gefunden.");

            var held = await _kontext.SuperHelden.FindAsync(ID);
            if (held == null)
                return BadRequest("Der Held wurde nicht gefunden.");
            return Ok(held);
        }

        [HttpPost]
        public async Task<ActionResult<List<SuperHeld>>> HeldEinzuFügen(SuperHeld held)
        {
            if (_kontext.SuperHelden == null)
                return NotFound("Keine SuperHelden gefunden.");

            _kontext.SuperHelden.Add(held);
            await _kontext.SaveChangesAsync();

            return Ok(await _kontext.SuperHelden.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<SuperHeld>>> HeldAktualisieren(SuperHeld anfrage)
        {
            if (_kontext.SuperHelden == null)
                return NotFound("Keine SuperHelden gefunden.");

            var dbHeld = await _kontext.SuperHelden.FindAsync(anfrage.ID);
            if (dbHeld == null)
                return BadRequest("Der Held wurde nicht gefunden.");

            dbHeld.Name = anfrage.Name;
            dbHeld.VorName = anfrage.VorName;
            dbHeld.NachName = anfrage.NachName;
            dbHeld.Ort = anfrage.Ort;

            await _kontext.SaveChangesAsync();

            return Ok(await _kontext.SuperHelden.ToListAsync());
        }

        [HttpDelete("{ID}")]
        public async Task<ActionResult<List<SuperHeld>>> HeldLöschen(int ID)
        {
            if (_kontext.SuperHelden == null)
                return NotFound("Keine SuperHelden gefunden.");

            var dbHeld = await _kontext.SuperHelden.FindAsync(ID);
            if (dbHeld == null)
                return BadRequest("Der Held wurde nicht gefunden.");

            _kontext.SuperHelden.Remove(dbHeld);
            await _kontext.SaveChangesAsync();

            return Ok(await _kontext.SuperHelden.ToListAsync());
        }

    }
}

