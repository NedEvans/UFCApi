using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UFCApi.CSVObjects;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace UFCApi.DB
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

        // GET /rounds
        [HttpGet]
        public async Task<IActionResult> GetRounds(
            [FromQuery] string? fightId,
            [FromQuery] string? fighterId,
            [FromQuery] int? round,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 100)
        {
            var query = _context.RoundsCsv.AsQueryable();

            if (!string.IsNullOrEmpty(fightId))
                query = query.Where(r => r.FightId == fightId);

            if (!string.IsNullOrEmpty(fighterId))
                query = query.Where(r => r.FighterId == fighterId);

            if (round.HasValue)
                query = query.Where(r => r.Round == round);

            var total = await query.CountAsync();
            var rounds = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new { total, page, pageSize, results = rounds });
        }

        // GET /rounds/{fightId}/{fighterId}/{round}
        [HttpGet("{fightId}/{fighterId}/{round}")]
        public async Task<IActionResult> GetRound(string fightId, string fighterId, int round)
        {
            var aRound = await _context.RoundsCsv.FindAsync(fightId, fighterId, round);

            if (aRound == null)
                return NotFound();

            return Ok(aRound);
        }

        // POST /rounds
        [HttpPost]
        public async Task<IActionResult> CreateRound(RoundCsv aRound)
        {
            // Ensure referenced fight and fighter exist
            var fightExists = await _context.FightsCsv.AnyAsync(f => f.FightId == aRound.FightId);
            var fighterExists = await _context.FightersCsv.AnyAsync(f => f.FighterId == aRound.FighterId);

            if (!fightExists || !fighterExists)
                return BadRequest("Invalid FightId or FighterId");

            // Enforce "unknown" on empty strings
            if (string.IsNullOrWhiteSpace(aRound.CtrlTime))
                aRound.CtrlTime = "unknown";

            _context.RoundsCsv.Add(aRound);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRound),
                new { fightId = aRound.FightId, fighterId = aRound.FighterId, round = aRound.Round },
                aRound);
        }

        // PUT /rounds/{fightId}/{fighterId}/{round}
        [HttpPut("{fightId}/{fighterId}/{round}")]
        public async Task<IActionResult> UpdateRound(string fightId, string fighterId, int round, RoundCsv aRound)
        {
            if (fightId != aRound.FightId || fighterId != aRound.FighterId || round != aRound.Round)
                return BadRequest();

            // Enforce "unknown" on empty strings
            if (string.IsNullOrWhiteSpace(aRound.CtrlTime))
                aRound.CtrlTime = "unknown";

            _context.Entry(aRound).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.RoundsCsv.Any(e => e.FightId == fightId && e.FighterId == fighterId && e.Round == round))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE /rounds/{fightId}/{fighterId}/{round}
        [HttpDelete("{fightId}/{fighterId}/{round}")]
        public async Task<IActionResult> DeleteRound(string fightId, string fighterId, int round)
        {
            var aRound = await _context.RoundsCsv.FindAsync(fightId, fighterId, round);
            if (aRound == null)
                return NotFound();

            _context.RoundsCsv.Remove(aRound);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
