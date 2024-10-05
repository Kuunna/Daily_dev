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

        public ItemController(HttpClient httpClient, ItemRepository itemRepository)
        {
            _httpClient = httpClient;
            _itemRepository = itemRepository;
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

        [HttpGet("get-rss")]
        public async Task<IActionResult> GetRssFeed(string rssUrl)
        {
            try
            {
                var response = await _httpClient.GetAsync(rssUrl);
                response.EnsureSuccessStatusCode();

                // Đọc dữ liệu XML từ RSS
                var rssData = await response.Content.ReadAsStringAsync();
                var rssXml = XDocument.Parse(rssData);

                // Phân tích và lưu dữ liệu
                ParseAndSaveRss(rssXml);

                return Ok("RSS data saved successfully");
            }
            catch (HttpRequestException e)
            {
                return BadRequest($"Error fetching RSS feed: {e.Message}");
            }
        }
        private void ParseAndSaveRss(XDocument rssXml)
        {
            // Lấy tất cả các phần tử <item> trong RSS
            var items = rssXml.Descendants("item");
            foreach (var item in items)
            {
                var title = item.Element("title")?.Value;
                var link = item.Element("link")?.Value;
                var guid = item.Element("guid")?.Value;
                var pubDate = item.Element("pubDate")?.Value;
                var author = item.Element("author")?.Value;
                var summary = item.Element("description")?.Value;
                var comments = item.Element("comments")?.Value;

                // Lấy hình ảnh từ thẻ <enclosure>
                var enclosure = item.Element("enclosure");
                var imageUrl = enclosure?.Attribute("url")?.Value; // Lấy giá trị của thuộc tính url

                // Chuyển đổi pubDate thành DateTime
                var parsedDate = ParseRssDate(pubDate);

                // Tạo đối tượng Item từ dữ liệu RSS
                var newItem = new Item
                {
                    Title = title,
                    Link = link,
                    Guid = guid,
                    PubDate = parsedDate,
                    Image = imageUrl ?? "Null", // Sử dụng giá trị "Null" nếu không có ảnh
                    CategoryId = 1, // Giả định một CategoryId cho ví dụ này
                    Author = author ?? "Null",
                    Summary = summary ?? "Null",
                    Comments = comments ?? "Null"
                };

                // Lưu vào cơ sở dữ liệu
                _itemRepository.Add(newItem);
            }
        }

        private DateTime ParseRssDate(string dateString)
        {
            // Xử lý chuỗi ngày giờ và các định dạng có thể
            string[] formats = {
                "ddd, dd MMM yyyy HH:mm:ss 'GMT'K",   // Định dạng có GMT+7
                "ddd, dd MMM yyyy HH:mm:ss 'GMT'zzz",  // Định dạng có GMT và zzz
                "ddd, dd MMM yyyy HH:mm:ss zzz",       // Định dạng chỉ có +07:00
                "ddd, dd MMM yyyy HH:mm:ss K",         // Định dạng chung có hoặc không có GMT
                "ddd, dd MMM yyyy HH:mm:ss +7",       // Định dạng với +7
                "ddd, dd MMM yyyy HH:mm:ss -7",       // Định dạng với -7
            };

            // Nếu dateString có chứa 'GMT', ta sẽ xóa nó
            dateString = dateString.Replace("GMT", "").Trim();

            // Chuyển đổi cú pháp của múi giờ từ +7 thành +07:00
            if (dateString.EndsWith("+7"))
            {
                dateString = dateString.Replace("+7", "+07:00");
            }
            else if (dateString.EndsWith("-7"))
            {
                dateString = dateString.Replace("-7", "-07:00");
            }

            // Phân tích cú pháp ngày giờ
            if (DateTimeOffset.TryParseExact(dateString, formats, null, System.Globalization.DateTimeStyles.None, out var dateTimeOffset))
            {
                return dateTimeOffset.UtcDateTime;
            }

            throw new FormatException($"Unable to parse date: {dateString}");
        }
    }
}
