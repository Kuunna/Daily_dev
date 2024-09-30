using daily_dev.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace daily_dev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookmarkController : ControllerBase
    {
        private readonly NewsDbContext _context;

        public BookmarkController(NewsDbContext context)
        {
            _context = context;
        }

        // GET: api/bookmark
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fact_Bookmark>>> GetAllBookmarks()
        {
            return await _context.Fact_Bookmark.ToListAsync();
        }

        // GET: api/bookmark/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Fact_Bookmark>>> GetBookmarksByUserId(int userId)
        {
            var bookmarks = await _context.Fact_Bookmark
                .Where(b => b.UserID == userId)
                .ToListAsync();

            if (!bookmarks.Any())
            {
                return NotFound();
            }

            return bookmarks;
        }

        // POST: api/bookmark
        [HttpPost]
        public async Task<ActionResult<Fact_Bookmark>> CreateBookmark([FromBody] Fact_Bookmark bookmark)
        {
            if (bookmark == null)
            {
                return BadRequest("Bookmark data is null");
            }

            _context.Fact_Bookmark.Add(bookmark);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookmarksByUserId), new { userId = bookmark.UserID }, bookmark);
        }

        // DELETE: api/bookmark/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookmark(int id)
        {
            var bookmark = await _context.Fact_Bookmark.FindAsync(id);
            if (bookmark == null)
            {
                return NotFound();
            }

            _context.Fact_Bookmark.Remove(bookmark);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/bookmark/user/{userId}/news/{newsId}
        [HttpDelete("user/{userId}/news/{newsId}")]
        public async Task<IActionResult> DeleteBookmarkByUserAndNews(int userId, int newsId)
        {
            var bookmark = await _context.Fact_Bookmark
                .Where(b => b.UserID == userId && b.NewsID == newsId)
                .FirstOrDefaultAsync();

            if (bookmark == null)
            {
                return NotFound();
            }

            _context.Fact_Bookmark.Remove(bookmark);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
