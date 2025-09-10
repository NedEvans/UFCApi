using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AzureAPI.Objects;

namespace AzureAPI.DB
{
    [ApiController]
    [Route("api/[controller]")]
    public class FighterController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FighterController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Fighter>> GetAll()
        {
            return await _context.Fighters.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Fighter>> Get(int id)
        {
            var fighter = await _context.Fighters.FindAsync(id);
            if (fighter == null) return NotFound();
            return fighter;
        }

        [HttpPost]
        public async Task<ActionResult<Fighter>> Create(Fighter fighter)
        {
            _context.Fighters.Add(fighter);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = fighter.Id }, fighter);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Fighter fighter)
        {
            if (id != fighter.Id) return BadRequest();
            _context.Entry(fighter).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var fighter = await _context.Fighters.FindAsync(id);
            if (fighter == null) return NotFound();
            _context.Fighters.Remove(fighter);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
