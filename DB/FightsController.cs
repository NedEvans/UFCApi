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
    public class FightsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FightsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /fights
        [HttpGet]
        public async Task<IActionResult> GetFights(
            [FromQuery] string? eventId,
            [FromQuery] string? fighterId,
            [FromQuery] DateTime? dateFrom,
            [FromQuery] DateTime? dateTo,
            [FromQuery] string? weightClass,
            [FromQuery] int? round,
            [FromQuery] string? result,
            [FromQuery] bool? titleFight,
            [FromQuery] string? referee,
            [FromQuery] string? sortBy = "EventDate",
            [FromQuery] string? order = "desc",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            var query = _context.FightsCsv
                .Include(f => f.Event)
                .AsQueryable();

            // Filters
            if (!string.IsNullOrEmpty(eventId))
                query = query.Where(f => f.EventId == eventId);

            if (!string.IsNullOrEmpty(fighterId))
                query = query.Where(f => f.Fighter1Id == fighterId || f.Fighter2Id == fighterId);

            if (dateFrom.HasValue)
                query = query.Where(f => f.Event != null && f.Event.EventDate >= dateFrom);

            if (dateTo.HasValue)
                query = query.Where(f => f.Event != null && f.Event.EventDate <= dateTo);

            if (!string.IsNullOrEmpty(weightClass))
                query = query.Where(f => EF.Functions.Like(f.WeightClass, $"%{weightClass}%"));

            if (round.HasValue)
                query = query.Where(f => f.FinishRound == round);

            if (!string.IsNullOrEmpty(result))
                query = query.Where(f => EF.Functions.Like(f.Result, $"%{result}%"));

            if (titleFight.HasValue)
                query = query.Where(f => f.TitleFight == titleFight);

            if (!string.IsNullOrEmpty(referee))
                query = query.Where(f => EF.Functions.Like(f.Referee, $"%{referee}%"));

            // Sorting
            query = (sortBy?.ToLower(), order?.ToLower()) switch
            {
                ("eventdate", "asc") => query.OrderBy(f => f.Event!.EventDate),
                ("eventdate", "desc") => query.OrderByDescending(f => f.Event!.EventDate),
                ("weightclass", "asc") => query.OrderBy(f => f.WeightClass),
                ("weightclass", "desc") => query.OrderByDescending(f => f.WeightClass),
                ("round", "asc") => query.OrderBy(f => f.FinishRound),
                ("round", "desc") => query.OrderByDescending(f => f.FinishRound),
                _ => query.OrderByDescending(f => f.Event!.EventDate) // default
            };

            // Pagination
            var totalCount = await query.CountAsync();
            var fights = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Results = fights
            });
        }

        // GET: /fights/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFight(string id)
        {
            var fight = await _context.FightsCsv
                .Include(f => f.Event)
                .FirstOrDefaultAsync(f => f.FightId == id);

            if (fight == null)
            {
                return NotFound();
            }

            return Ok(fight);
        }

        // POST: /fights
        [HttpPost]
        public async Task<IActionResult> CreateFight(FightCsv fight)
        {
            _context.FightsCsv.Add(fight);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFight), new { id = fight.FightId }, fight);
        }

        // PUT: /fights/{id}
        [HttpPut("{id}")]
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

        // DELETE: /fights/{id}
        [HttpDelete("{id}")]
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
