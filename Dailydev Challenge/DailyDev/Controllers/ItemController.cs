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
        private readonly HttpClient _httpClient;
        private readonly ItemRepository _itemRepository;
        private readonly CategoryRepository _categoryRepository;

        public ItemController(HttpClient httpClient, ItemRepository itemRepository, CategoryRepository categoryRepository)
        {
            _httpClient = httpClient;
            _itemRepository = itemRepository;
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
        public async Task<IActionResult> FetchAllRssFeeds()
        {
            try
            {
                // Lấy tất cả Category từ bảng Category
                var categories = _categoryRepository.GetAll();

                foreach (var category in categories)
                {
                    // Lấy dữ liệu RSS từ từng Category
                    var response = await _httpClient.GetAsync(category.Source);
                    response.EnsureSuccessStatusCode();

                    var rssData = await response.Content.ReadAsStringAsync();
                    var rssXml = XDocument.Parse(rssData);

                    // Phân tích và lưu dữ liệu RSS vào bảng Item
                    _itemRepository.ParseAndSaveRss(rssXml, category.Id);
                }

                return Ok("RSS data fetched and saved successfully");
            }
            catch (HttpRequestException e)
            {
                return BadRequest($"Error fetching RSS feed: {e.Message}");
            }
        }
    }
}
