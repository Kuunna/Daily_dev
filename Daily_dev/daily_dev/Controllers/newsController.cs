using daily_dev.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace daily_dev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly NewsDbContext _context;

        public NewsController(NewsDbContext context)
        {
            _context = context;
        }

        // GET: api/news
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fact_News>>> GetAllNews()
        {
            return await _context.Fact_News.ToListAsync();
        }

        // GET: api/news/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Fact_News>> GetNewsById(int id)
        {
            var newsItem = await _context.Fact_News.FindAsync(id);

            if (newsItem == null)
            {
                return NotFound();
            }

            return newsItem;
        }

        // POST: api/news
        [HttpPost]
        public async Task<ActionResult<Fact_News>> CreateNews([FromBody] Fact_News news)
        {
            if (news == null)
            {
                return BadRequest("News data is null");
            }

            _context.Fact_News.Add(news);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNewsById), new { id = news.NewID }, news);
        }

        // PUT: api/news/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNews(int id, [FromBody] Fact_News news)
        {
            if (id != news.NewID)
            {
                return BadRequest("News ID mismatch");
            }

            var existingNews = await _context.Fact_News.FindAsync(id);
            if (existingNews == null)
            {
                return NotFound();
            }

            existingNews.Title = news.Title;
            existingNews.Content = news.Content;
            existingNews.PublishedDate = news.PublishedDate;
            existingNews.SourceID = news.SourceID;
            existingNews.TopicID = news.TopicID;
            existingNews.Author = news.Author;
            existingNews.ImageURL = news.ImageURL;
            existingNews.ViewCount = news.ViewCount;
            existingNews.LikeCount = news.LikeCount;
            existingNews.CommentCount = news.CommentCount;

            _context.Fact_News.Update(existingNews);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/news/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var newsItem = await _context.Fact_News.FindAsync(id);
            if (newsItem == null)
            {
                return NotFound();
            }

            _context.Fact_News.Remove(newsItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
