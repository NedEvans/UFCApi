using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UFCApi.CSVObjects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace UFCApi.DB
{
    [ApiController]
    [Route("[controller]")]
    public class FightersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FightersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /fighters
        [HttpGet]
        public async Task<IActionResult> GetFighters(
            [FromQuery] string? firstName,
            [FromQuery] string? lastName,
            [FromQuery] string? nickname,
            [FromQuery] string? stance,
            [FromQuery] int? minWins,
            [FromQuery] int? maxWins,
            [FromQuery] int? minLosses,
            [FromQuery] int? maxLosses,
            [FromQuery] int? minDraws,
            [FromQuery] int? maxDraws,
            [FromQuery] int? minNcDq,
            [FromQuery] int? maxNcDq,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            var query = _context.FightersCsv.AsQueryable();

            // String filters
            if (!string.IsNullOrEmpty(firstName))
                query = query.Where(f => EF.Functions.Like(f.FighterFName, $"%{firstName}%"));

            if (!string.IsNullOrEmpty(lastName))
                query = query.Where(f => EF.Functions.Like(f.FighterLName, $"%{lastName}%"));

            if (!string.IsNullOrEmpty(nickname))
                query = query.Where(f => EF.Functions.Like(f.FighterNickname, $"%{nickname}%"));

            if (!string.IsNullOrEmpty(stance))
                query = query.Where(f => EF.Functions.Like(f.FighterStance, $"%{stance}%"));

            // Numeric filters
            if (minWins.HasValue)
                query = query.Where(f => f.FighterW >= minWins.Value);
            if (maxWins.HasValue)
                query = query.Where(f => f.FighterW <= maxWins.Value);

            if (minLosses.HasValue)
                query = query.Where(f => f.FighterL >= minLosses.Value);
            if (maxLosses.HasValue)
                query = query.Where(f => f.FighterL <= maxLosses.Value);

            if (minDraws.HasValue)
                query = query.Where(f => f.FighterD >= minDraws.Value);
            if (maxDraws.HasValue)
                query = query.Where(f => f.FighterD <= maxDraws.Value);

            if (minNcDq.HasValue)
                query = query.Where(f => f.FighterNcDq >= minNcDq.Value);
            if (maxNcDq.HasValue)
                query = query.Where(f => f.FighterNcDq <= maxNcDq.Value);

            return Ok(await query.ToListAsync());
        }

        // GET: /fighters/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFighter(string id)
        {
            var fighter = await _context.FightersCsv.FindAsync(id);

            if (fighter == null)
            {
                return NotFound();
            }

            return Ok(fighter);
        }

        // POST: /fighters
        [HttpPost]
        public async Task<IActionResult> CreateFighter(FighterCsv fighter)
        {
            _context.FightersCsv.Add(fighter);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFighter), new { id = fighter.FighterId }, fighter);
        }

        // PUT: /fighters/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFighter(string id, FighterCsv fighter)
        {
            if (id != fighter.FighterId)
            {
                return BadRequest();
            }

            _context.Entry(fighter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.FightersCsv.Any(e => e.FighterId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: /fighters/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFighter(string id)
        {
            var fighter = await _context.FightersCsv.FindAsync(id);
            if (fighter == null)
            {
                return NotFound();
            }

            _context.FightersCsv.Remove(fighter);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
