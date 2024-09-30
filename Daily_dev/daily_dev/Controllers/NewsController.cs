using System.Xml;
using System.ServiceModel.Syndication;
using Microsoft.AspNetCore.Mvc;
using daily_dev.Models; 
using System.Collections.Generic;
using System.Linq;
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

        [HttpPost("import")]
        public async Task<IActionResult> ImportRssFeeds()
        {
            var rssUrls = new List<string>
            {
                "https://baomoi.com/rss",  // Ví dụ URL của Báo Mới
                "https://tuoitre.vn/rss",   // Ví dụ URL của Tuổi Trẻ
                "https://dantri.com.vn/rss"  // Ví dụ URL của Dân Trí
            };

            foreach (var url in rssUrls)
            {
                using (var reader = XmlReader.Create(url))
                {
                    var feed = SyndicationFeed.Load(reader);
                    foreach (var item in feed.Items)
                    {
                        // Tìm SourceID dựa trên tên nguồn
                        var source = _context.Dim_Source.FirstOrDefault(s => s.SourceName == url);
                        int sourceId = source?.SourceID ?? 0; // Gán giá trị mặc định nếu không tìm thấy

                        // Gán TopicID mặc định (cần tạo topic trước)
                        int topicId = 1; // Giả định đã có topic mặc định với ID = 1

                        var news = new Fact_News
                        {
                            Title = item.Title.Text,
                            Content = item.Summary.Text,
                            PublishedDate = (int)(item.PublishDate.UtcDateTime.ToFileTimeUtc() / 10000), // Chuyển đổi về kiểu int
                            SourceID = sourceId,
                            TopicID = topicId,
                            Author = item.ElementExtensions.ReadElementExtensions<string>("author", "dc").FirstOrDefault() ?? "Unknown", // Thay đổi theo cách bạn muốn lấy tác giả
                            ImageURL = item.ElementExtensions.ReadElementExtensions<string>("image", "media").FirstOrDefault(),
                            ViewCount = 0,
                            LikeCount = 0,
                            CommentCount = 0
                        };

                        _context.Fact_News.Add(news);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return Ok("Imported RSS feeds successfully");
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
