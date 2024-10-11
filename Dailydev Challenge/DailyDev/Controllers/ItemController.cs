using DailyDev.Models;
using DailyDev.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DailyDev.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly ItemRepository _itemRepository;
        private readonly HttpClient _httpClient;
        private readonly CategoryRepository _categoryRepository;

        public ItemController(ItemRepository itemRepository, HttpClient httpClient, CategoryRepository categoryRepository)
        {
            _itemRepository = itemRepository;
            _httpClient = httpClient;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Item>> GetAll()
        {
            return Ok(_itemRepository.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Item> GetById(int id)
        {
            var item = _itemRepository.GetById(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public ActionResult Add([FromBody] Item item)
        {
            _itemRepository.Add(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Item item)
        {
            item.Id = id;
            _itemRepository.Update(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _itemRepository.Delete(id);
            return NoContent();
        }

        // Method getall RSS feed from Category and save data Item
        [HttpGet("fetch-all-rss")]
        public async Task<IActionResult> FetchAllRssFeeds(CancellationToken cancellationToken)
        {
            try
            {
                var categories = _categoryRepository.GetAll();
                int batchSize = 10;  // Số lượng category mỗi batch
                _httpClient.Timeout = TimeSpan.FromMinutes(5); // Tăng timeout cho HttpClient 5 phút

                for (int i = 0; i < categories.Count(); i += batchSize)
                {
                    var batchCategories = categories.Skip(i).Take(batchSize);

                    // Thực hiện song song các yêu cầu trong batch với Task.WhenAll
                    var tasks = batchCategories.Select(async category =>
                    {
                        var response = await _httpClient.GetAsync(category.Source, cancellationToken);
                        response.EnsureSuccessStatusCode();

                        var rssData = await response.Content.ReadAsStringAsync();
                        var rssXml = XDocument.Parse(rssData);

                        // Phân tích và lưu dữ liệu RSS vào bảng Item
                        _itemRepository.ParseAndSaveRss(rssXml, category.Id);
                    });

                    await Task.WhenAll(tasks); // Chờ tất cả các task trong batch hoàn thành
                }

                return Ok("RSS data fetched and saved successfully");
            }
            catch (HttpRequestException e)
            {
                return BadRequest($"Error fetching RSS feed: {e.Message}");
            }
            catch (OperationCanceledException)
            {
                return StatusCode(408, "Request timed out.");
            }
        }
    }
}
