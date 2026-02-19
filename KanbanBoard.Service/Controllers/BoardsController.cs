using KanbanBoardService.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoardService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardsController : ControllerBase
    {
        private readonly KanbanBoardDatabaseContext _db;

        public BoardsController(KanbanBoardDatabaseContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Board>>> GetBoards()
        {
            return await _db.Boards.Include(b => b.Phases).ToListAsync();
        }

        [HttpGet("{id}", Name = "GetBoard")]
        public async Task<ActionResult<Board>> GetBoard(int id)
        {
            var board = await _db.Boards.Include(b => b.Phases).FirstOrDefaultAsync(b => b.Id == id);
            if (board == null) return NotFound();
            return board;
        }

        [HttpPost]
        public async Task<ActionResult<Board>> CreateBoard(Board board)
        {
            board.Id = 0;
            _db.Boards.Add(board);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetBoard", new { id = board.Id }, board);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Board>> UpdateBoard(int id, Board board)
        {
            try
            {
                if (id != board.Id) return BadRequest();
                _db.Boards.Update(board);
                await _db.SaveChangesAsync();
                return Ok(board);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _db.Boards.AnyAsync(e => e.Id == id)) return NotFound();
                return StatusCode(500, "Failed to update board");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoard(int id)
        {
            var board = await _db.Boards.FindAsync(id);
            if (board == null) return NotFound();
            _db.Boards.Remove(board);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}