using daily_dev.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace daily_dev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly NewsDbContext _context;

        public HistoryController(NewsDbContext context)
        {
            _context = context;
        }

        // GET: api/history
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fact_History>>> GetAllHistories()
        {
            return await _context.Fact_History.ToListAsync();
        }

        // GET: api/history/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Fact_History>>> GetHistoryByUserId(int userId)
        {
            var histories = await _context.Fact_History
                .Where(h => h.UserID == userId)
                .ToListAsync();

            if (!histories.Any())
            {
                return NotFound();
            }

            return histories;
        }

        // GET: api/history/user/{userId}/news/{newsId}
        [HttpGet("user/{userId}/news/{newsId}")]
        public async Task<ActionResult<Fact_History>> GetHistoryByUserAndNews(int userId, int newsId)
        {
            var history = await _context.Fact_History
                .FirstOrDefaultAsync(h => h.UserID == userId && h.NewsID == newsId);

            if (history == null)
            {
                return NotFound();
            }

            return history;
        }

        // POST: api/history
        [HttpPost]
        public async Task<ActionResult<Fact_History>> CreateHistory([FromBody] Fact_History history)
        {
            if (history == null)
            {
                return BadRequest("History data is null");
            }

            _context.Fact_History.Add(history);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHistoryByUserId), new { userId = history.UserID }, history);
        }

        // DELETE: api/history/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistory(int id)
        {
            var history = await _context.Fact_History.FindAsync(id);
            if (history == null)
            {
                return NotFound();
            }

            _context.Fact_History.Remove(history);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/history/user/{userId}/news/{newsId}
        [HttpDelete("user/{userId}/news/{newsId}")]
        public async Task<IActionResult> DeleteHistoryByUserAndNews(int userId, int newsId)
        {
            var history = await _context.Fact_History
                .Where(h => h.UserID == userId && h.NewsID == newsId)
                .FirstOrDefaultAsync();

            if (history == null)
            {
                return NotFound();
            }

            _context.Fact_History.Remove(history);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
