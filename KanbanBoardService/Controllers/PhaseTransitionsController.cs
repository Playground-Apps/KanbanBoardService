using KanbanBoardService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoardService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhaseTransitionsController : ControllerBase
    {
        private readonly KanbanBoardDatabaseContext _db;

        public PhaseTransitionsController(KanbanBoardDatabaseContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhaseTransitions>>> GetTransitions()
        {
            var result = await _db.PhaseTransitions
                .Include(pt => pt.FromPhase)
                .Include(pt => pt.ToPhase)
                .ToListAsync();
            if (result == null || result.Count == 0)
            {
                return NoContent();
            }
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetTransition")]
        public async Task<ActionResult<PhaseTransitions>> GetTransition(int id)
        {
            var t = await _db.PhaseTransitions
                .Include(pt => pt.FromPhase)
                .Include(pt => pt.ToPhase)
                .FirstOrDefaultAsync(pt => pt.Id == id);

            if (t == null) return NotFound();
            return Ok(t);
        }

        [HttpPost]
        public async Task<ActionResult<PhaseTransitions>> CreateTransition(PhaseTransitions transition)
        {
            _db.PhaseTransitions.Add(transition);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetTransition", new { id = transition.Id }, transition);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PhaseTransitions>> UpdateTransition(int id, PhaseTransitions transition)
        {
            try
            {
                if (id != transition.Id) return BadRequest();
                _db.PhaseTransitions.Update(transition);
                await _db.SaveChangesAsync();
                return Ok(transition);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _db.PhaseTransitions.AnyAsync(e => e.Id == id)) return NotFound();
            return StatusCode(500,"Failed to move task");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransition(int id)
        {
            var t = await _db.PhaseTransitions.FindAsync(id);
            if (t == null) return NotFound();
            _db.PhaseTransitions.Remove(t);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}