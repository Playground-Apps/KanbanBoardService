using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = KanbanBoardService.Models.Task;

namespace KanbanBoardService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly KanbanBoardDatabaseContext _db;

        public TasksController(KanbanBoardDatabaseContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Task>>> GetTasks()
        {
            return await _db.Tasks.Include(t => t.Phase).ToListAsync();
        }

        [HttpGet("{id}", Name = "GetTask")]
        public async Task<ActionResult<Task>> GetTask(int id)
        {
            var item = await _db.Tasks.Include(t => t.Phase).FirstOrDefaultAsync(t => t.Id == id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Task>> CreateTask(Task task)
        {
            task.CreatedAt = task.UpdatedAt = System.DateTime.UtcNow;
            _db.Tasks.Add(task);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetTask", new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Task>> UpdateTask(int id, Task task)
        {
            try
            {
                if (id != task.Id) return BadRequest();

                task.UpdatedAt = System.DateTime.UtcNow;
                _db.Tasks.Update(task);
                await _db.SaveChangesAsync();
                return Ok(task);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _db.Tasks.AnyAsync(e => e.Id == id)) return NotFound();
                return StatusCode(500, "Failed to update task");
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var item = await _db.Tasks.FindAsync(id);
            if (item == null) return NotFound();

            _db.Tasks.Remove(item);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}