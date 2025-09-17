using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UFCApi.CSVObjects;


namespace UFCApi.DB
{
    [ApiController]
    [Route("events")]
    public class EventsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EventsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetEvents(
            [FromQuery] string? name,
            [FromQuery] DateTime? dateFrom,
            [FromQuery] DateTime? dateTo,
            [FromQuery] string? city,
            [FromQuery] string? state,
            [FromQuery] string? country)
        {
            var query = _context.EventsCsv.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(e => e.EventName != null && EF.Functions.Like(e.EventName, $"%{name}%"));
            if (dateFrom.HasValue)
                query = query.Where(e => e.EventDate >= dateFrom);
            if (dateTo.HasValue)
                query = query.Where(e => e.EventDate <= dateTo);
            if (!string.IsNullOrEmpty(city))
                query = query.Where(e => e.EventCity != null && EF.Functions.Like(e.EventCity, $"%{city}%"));
            if (!string.IsNullOrEmpty(state))
                query = query.Where(e => e.EventState != null && EF.Functions.Like(e.EventState, $"%{state}%"));
            if (!string.IsNullOrEmpty(country))
                query = query.Where(e => e.EventCountry != null && EF.Functions.Like(e.EventCountry, $"%{country}%"));

            return Ok(await query.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(string id)
        {
            var anEvent = await _context.EventsCsv.FindAsync(id);
            return anEvent == null ? NotFound() : Ok(anEvent);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] EventCsv anEvent)
        {
            if (string.IsNullOrWhiteSpace(anEvent.EventId))
                return BadRequest("EventId cannot be empty.");

            _context.EventsCsv.Add(anEvent);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEvent), new { id = anEvent.EventId }, anEvent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(string id, [FromBody] EventCsv anEvent)
        {
            if (id != anEvent.EventId) return BadRequest();

            _context.Entry(anEvent).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.EventsCsv.Any(e => e.EventId == id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(string id)
        {
            var anEvent = await _context.EventsCsv.FindAsync(id);
            if (anEvent == null) return NotFound();

            _context.EventsCsv.Remove(anEvent);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("fix-empty-events")]
        public async Task<IActionResult> FixEmptyEvents()
        {
            var events = await _context.EventsCsv.ToListAsync();
            var updatedCount = 0;

            foreach (var e in events)
            {
                var updated = false;

                if (string.IsNullOrWhiteSpace(e.EventName))
                {
                    e.EventName = "unknown";
                    updated = true;
                }
                if (string.IsNullOrWhiteSpace(e.EventCity))
                {
                    e.EventCity = "unknown";
                    updated = true;
                }
                if (string.IsNullOrWhiteSpace(e.EventState))
                {
                    e.EventState = "unknown";
                    updated = true;
                }
                if (string.IsNullOrWhiteSpace(e.EventCountry))
                {
                    e.EventCountry = "unknown";
                    updated = true;
                }

                if (updated)
                {
                    updatedCount++;
                }
            }

            if (updatedCount > 0)
            {
                await _context.SaveChangesAsync();
            }

            return Ok(new
            {
                Message = "Empty event fields fixed.",
                UpdatedEvents = updatedCount,
                TotalEvents = events.Count
            });
        }


    }

}
