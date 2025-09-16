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
    public class FightsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FightsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("fights")]
        public async Task<IActionResult> GetFights(
            [FromQuery] string? EventId,
            [FromQuery] string? FighterId,
            [FromQuery] DateTime? DateFrom,
            [FromQuery] DateTime? DateTo,
            [FromQuery] string? WeightClass,
            [FromQuery] int? Round,
            [FromQuery] string? Result)
        {
            var query = _context.FightsCsv.AsQueryable();

            if (!string.IsNullOrEmpty(EventId))
                query = query.Where(f => f.EventId == EventId);

            if (!string.IsNullOrEmpty(FighterId))
                query = query.Where(f => f.Fighter1Id == FighterId || f.Fighter2Id == FighterId);

            if (DateFrom.HasValue)
                query = query.Where(f => f.Event != null && f.Event.EventDate >= DateFrom);

            if (DateTo.HasValue)
                query = query.Where(f => f.Event != null && f.Event.EventDate <= DateTo);

            if (!string.IsNullOrEmpty(WeightClass))
                query = query.Where(f => f.WeightClass == WeightClass);

            if (Round.HasValue)
                query = query.Where(f => f.FinishRound == Round);

            if (!string.IsNullOrEmpty(Result))
                query = query.Where(f => f.Result == Result);

            var fights = await query.ToListAsync();
            return Ok(fights);
        }

        [HttpGet("fights/{id}")]
        public async Task<IActionResult> GetFight(string id)
        {
            var fight = await _context.FightsCsv.FindAsync(id);

            if (fight == null)
            {
                return NotFound();
            }

            return Ok(fight);
        }

        [HttpPost("fights")]
        public async Task<IActionResult> CreateFight(FightCsv fight)
        {
            _context.FightsCsv.Add(fight);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFight), new { id = fight.FightId }, fight);
        }

        [HttpPut("fights/{id}")]
        public async Task<IActionResult> UpdateFight(string id, FightCsv fight)
        {
            if (id != fight.FightId)
            {
                return BadRequest();
            }

            _context.Entry(fight).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.FightsCsv.Any(e => e.FightId == id))
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

        [HttpDelete("fights/{id}")]
        public async Task<IActionResult> DeleteFight(string id)
        {
            var fight = await _context.FightsCsv.FindAsync(id);
            if (fight == null)
            {
                return NotFound();
            }

            _context.FightsCsv.Remove(fight);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}