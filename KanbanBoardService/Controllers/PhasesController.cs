using KanbanBoardService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoardService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhasesController : ControllerBase
    {
        private readonly KanbanBoardDatabaseContext _db;

        public PhasesController(KanbanBoardDatabaseContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Phase>>> GetPhases()
        {
            return await _db.Phases.Include(p => p.Board).Include(p => p.Tasks).ToListAsync();
        }

        [HttpGet("{id}", Name = "GetPhase")]
        public async Task<ActionResult<Phase>> GetPhase(int id)
        {
            var phase = await _db.Phases
                .Include(p => p.Board)
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (phase == null) return NotFound();
            return phase;
        }

        [HttpGet("/phasesByBoard/{id}")]
        public async Task<ActionResult<List<Phase>>> GetPhasesByBoard(int id, [FromQuery] bool includeTask = false)
        {
            List<Phase> result = new List<Phase>();
            if (includeTask)
            {
                result = await _db.Phases
                    .Include(p => p.Board)
                    .Include(p => p.Tasks)
                    .Where(p => p.BoardId == id).ToListAsync();
            }
            else
            {
                result = await _db.Phases
                    .Include(p => p.Board)
                    .Where(p => p.BoardId == id).ToListAsync();
            }

            if (!result.Any()) return NoContent();
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<Phase>> CreatePhase(Phase phase)
        {
            _db.Phases.Add(phase);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetPhase", new { id = phase.Id }, phase);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Phase>> UpdatePhase(int id, Phase phase)
        {
            try
            {
                if (id != phase.Id) return BadRequest();
                _db.Phases.Update(phase);
                await _db.SaveChangesAsync();
                return Ok(phase);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _db.Phases.AnyAsync(e => e.Id == id)) return NotFound();
                return StatusCode(500, "Failed to update phase");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhase(int id)
        {
            var phase = await _db.Phases.FindAsync(id);
            if (phase == null) return NotFound();
            _db.Phases.Remove(phase);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}