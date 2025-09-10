using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AzureAPI.Objects;

namespace AzureAPI.DB
{
    [ApiController]
    [Route("api/[controller]")]
    public class FightController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FightController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Fight>> GetAll()
        {
            return await _context.Fights
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Fight>> Get(int id)
        {
            var fight = await _context.Fights
                .FirstOrDefaultAsync(f => f.Id == id);

            if (fight == null) return NotFound();
            return fight;
        }

        [HttpPost]
        public async Task<ActionResult<Fight>> Create(Fight fight)
        {
            _context.Fights.Add(fight);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = fight.Id }, fight);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Fight fight)
        {
            if (id != fight.Id) return BadRequest();
            _context.Entry(fight).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var fight = await _context.Fights.FindAsync(id);
            if (fight == null) return NotFound();
            _context.Fights.Remove(fight);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
