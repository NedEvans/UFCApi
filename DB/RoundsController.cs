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
    public class RoundsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RoundsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("rounds")]
        public async Task<IActionResult> GetRounds(
            [FromQuery] string? fightId,
            [FromQuery] string? fighterId)
        {
            var query = _context.RoundsCsv.AsQueryable();

            if (!string.IsNullOrEmpty(fightId))
                query = query.Where(r => r.FightId == fightId);

            if (!string.IsNullOrEmpty(fighterId))
                query = query.Where(r => r.FighterId == fighterId);

            var rounds = await query.ToListAsync();
            return Ok(rounds);
        }

        [HttpGet("rounds/{fightId}/{fighterId}/{round}")]
        public async Task<IActionResult> GetRound(string fightId, string fighterId, int round)
        {
            var aRound = await _context.RoundsCsv.FindAsync(fightId, fighterId, round);

            if (aRound == null)
            {
                return NotFound();
            }

            return Ok(aRound);
        }

        [HttpPost("rounds")]
        public async Task<IActionResult> CreateRound(RoundCsv aRound)
        {
            _context.RoundsCsv.Add(aRound);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRound), new { fightId = aRound.FightId, fighterId = aRound.FighterId, round = aRound.Round }, aRound);
        }

        [HttpPut("rounds/{fightId}/{fighterId}/{round}")]
        public async Task<IActionResult> UpdateRound(string fightId, string fighterId, int round, RoundCsv aRound)
        {
            if (fightId != aRound.FightId || fighterId != aRound.FighterId || round != aRound.Round)
            {
                return BadRequest();
            }

            _context.Entry(aRound).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.RoundsCsv.Any(e => e.FightId == fightId && e.FighterId == fighterId && e.Round == round))
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

        [HttpDelete("rounds/{fightId}/{fighterId}/{round}")]
        public async Task<IActionResult> DeleteRound(string fightId, string fighterId, int round)
        {
            var aRound = await _context.RoundsCsv.FindAsync(fightId, fighterId, round);
            if (aRound == null)
            {
                return NotFound();
            }

            _context.RoundsCsv.Remove(aRound);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
