using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DailyDev.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public NewsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Lấy RSS feed từ một URL cụ thể
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

                // Trả về XML thô cho dễ kiểm tra
                return Ok(rssXml);
            }
            catch (HttpRequestException e)
            {
                return BadRequest($"Error fetching RSS feed: {e.Message}");
            }

        }
    }
}