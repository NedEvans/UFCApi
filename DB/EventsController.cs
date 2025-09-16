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
    public class EventsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EventsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("events")]
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
                query = query.Where(e => e.EventName != null && e.EventName.Contains(name));

            if (dateFrom.HasValue)
                query = query.Where(e => e.EventDate >= dateFrom);

            if (dateTo.HasValue)
                query = query.Where(e => e.EventDate <= dateTo);

            if (!string.IsNullOrEmpty(city))
                query = query.Where(e => e.EventCity != null && e.EventCity.Contains(city));

            if (!string.IsNullOrEmpty(state))
                query = query.Where(e => e.EventState != null && e.EventState.Contains(state));

            if (!string.IsNullOrEmpty(country))
                query = query.Where(e => e.EventCountry != null && e.EventCountry.Contains(country));

            var events = await query.ToListAsync();
            return Ok(events);
        }

        [HttpGet("events/{id}")]
        public async Task<IActionResult> GetEvent(string id)
        {
            var anEvent = await _context.EventsCsv.FindAsync(id);

            if (anEvent == null)
            {
                return NotFound();
            }

            return Ok(anEvent);
        }

        [HttpPost("events")]
        public async Task<IActionResult> CreateEvent(EventCsv anEvent)
        {
            _context.EventsCsv.Add(anEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = anEvent.EventId }, anEvent);
        }

        [HttpPut("events/{id}")]
        public async Task<IActionResult> UpdateEvent(string id, EventCsv anEvent)
        {
            if (id != anEvent.EventId)
            {
                return BadRequest();
            }

            _context.Entry(anEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.EventsCsv.Any(e => e.EventId == id))
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

        [HttpDelete("events/{id}")]
        public async Task<IActionResult> DeleteEvent(string id)
        {
            var anEvent = await _context.EventsCsv.FindAsync(id);
            if (anEvent == null)
            {
                return NotFound();
            }

            _context.EventsCsv.Remove(anEvent);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
