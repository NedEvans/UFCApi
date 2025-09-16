using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AzureAPI.DB;
using UFCApi.CSVObjects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AzureAPI.Controllers
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

        [HttpGet("fighters")]
        public async Task<IActionResult> GetFighters(
            [FromQuery] string? firstName,
            [FromQuery] string? lastName,
            [FromQuery] string? nickname,
            [FromQuery] string? stance)
        {
            var query = _context.FightersCsv.AsQueryable();

            if (!string.IsNullOrEmpty(firstName))
                query = query.Where(f => f.FighterFName != null && f.FighterFName.Contains(firstName));

            if (!string.IsNullOrEmpty(lastName))
                query = query.Where(f => f.FighterLName != null && f.FighterLName.Contains(lastName));

            if (!string.IsNullOrEmpty(nickname))
                query = query.Where(f => f.FighterNickname != null && f.FighterNickname.Contains(nickname));

            if (!string.IsNullOrEmpty(stance))
                query = query.Where(f => f.FighterStance != null && f.FighterStance == stance);

            var fighters = await query.ToListAsync();
            return Ok(fighters);
        }

        [HttpGet("fighters/{id}")]
        public async Task<IActionResult> GetFighter(string id)
        {
            var fighter = await _context.FightersCsv.FindAsync(id);

            if (fighter == null)
            {
                return NotFound();
            }

            return Ok(fighter);
        }

        [HttpPost("fighters")]
        public async Task<IActionResult> CreateFighter(FighterCsv fighter)
        {
            _context.FightersCsv.Add(fighter);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFighter), new { id = fighter.FighterId }, fighter);
        }

        [HttpPut("fighters/{id}")]
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

        [HttpDelete("fighters/{id}")]
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
