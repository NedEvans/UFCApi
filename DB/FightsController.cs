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
        [HttpGet("fights")]
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
        [HttpGet("fights/{id}")]
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
        [HttpPost("fights")]
        public async Task<IActionResult> CreateFight(FightCsv fight)
        {
            _context.FightsCsv.Add(fight);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFight), new { id = fight.FightId }, fight);
        }

        // PUT: /fights/{id}
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

        // DELETE: /fights/{id}
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

        [HttpPost("diagnose-and-fix-fights")]
        public async Task<IActionResult> DiagnoseAndFixFights()
        {
            var fights = await _context.FightsCsv.ToListAsync();
            var fightsWithEmptyFields = new List<object>();

            foreach (var fight in fights)
            {
                var emptyFields = new List<string>();

                if (string.IsNullOrWhiteSpace(fight.Referee))
                {
                    fight.Referee = "unknown";
                    emptyFields.Add(nameof(fight.Referee));
                }

                if (string.IsNullOrWhiteSpace(fight.WeightClass))
                {
                    fight.WeightClass = "unknown";
                    emptyFields.Add(nameof(fight.WeightClass));
                }

                if (string.IsNullOrWhiteSpace(fight.Gender))
                {
                    fight.Gender = "unknown";
                    emptyFields.Add(nameof(fight.Gender));
                }

                if (string.IsNullOrWhiteSpace(fight.Result))
                {
                    fight.Result = "unknown";
                    emptyFields.Add(nameof(fight.Result));
                }

                if (string.IsNullOrWhiteSpace(fight.ResultDetails))
                {
                    fight.ResultDetails = "unknown";
                    emptyFields.Add(nameof(fight.ResultDetails));
                }

                if (string.IsNullOrWhiteSpace(fight.FinishTime))
                {
                    fight.FinishTime = "unknown";
                    emptyFields.Add(nameof(fight.FinishTime));
                }

                if (string.IsNullOrWhiteSpace(fight.TimeFormat))
                {
                    fight.TimeFormat = "unknown";
                    emptyFields.Add(nameof(fight.TimeFormat));
                }

                if (string.IsNullOrWhiteSpace(fight.Scores1))
                {
                    fight.Scores1 = "unknown";
                    emptyFields.Add(nameof(fight.Scores1));
                }

                if (string.IsNullOrWhiteSpace(fight.Scores2))
                {
                    fight.Scores2 = "unknown";
                    emptyFields.Add(nameof(fight.Scores2));
                }

                if (emptyFields.Any())
                {
                    fightsWithEmptyFields.Add(new
                    {
                        fight.FightId,
                        EmptyFields = emptyFields
                    });
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                TotalFights = fights.Count,
                FightsWithEmptyFields = fightsWithEmptyFields.Count,
                Examples = fightsWithEmptyFields.Take(10)
            });
        }

    }
}
