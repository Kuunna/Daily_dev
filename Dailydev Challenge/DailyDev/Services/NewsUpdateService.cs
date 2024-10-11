using DailyDev.Models;
using DailyDev.Repository;
using System.Xml.Linq;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DailyDev.Service
{
    public class NewsUpdateService : BackgroundService
    {
        private readonly HttpClient _httpClient;
        private readonly IServiceProvider _serviceProvider; // Thêm IServiceProvider
        private readonly ILogger<NewsUpdateService> _logger;

        public NewsUpdateService(
            HttpClient httpClient,
            IServiceProvider serviceProvider,
            ILogger<NewsUpdateService> logger)
        {
            _httpClient = httpClient;
            _serviceProvider = serviceProvider; // Lưu IServiceProvider
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Starting RSS feed update...");

                    await FetchAndUpdateNews(stoppingToken);

                    _logger.LogInformation("RSS feed update completed.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating RSS feeds.");
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        // Hàm lấy RSS từ các category và lưu vào database
        private async Task FetchAndUpdateNews(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope()) // Tạo một scope mới
            {
                var itemRepository = scope.ServiceProvider.GetRequiredService<ItemRepository>();
                var categoryRepository = scope.ServiceProvider.GetRequiredService<CategoryRepository>();

                var categories = categoryRepository.GetAll();
                int batchSize = 10;  // Số lượng category mỗi batch
                _httpClient.Timeout = TimeSpan.FromMinutes(5); // Tăng timeout cho HttpClient 5 phút

                for (int i = 0; i < categories.Count(); i += batchSize)
                {
                    var batchCategories = categories.Skip(i).Take(batchSize);

                    // Thực hiện song song các yêu cầu trong batch với Task.WhenAll
                    var tasks = batchCategories.Select(async category =>
                    {
                        _logger.LogInformation($"Fetching RSS feed for category: {category.Name}");

                        try
                        {
                            var response = await _httpClient.GetAsync(category.Source, cancellationToken);
                            response.EnsureSuccessStatusCode();

                            var rssData = await response.Content.ReadAsStringAsync();
                            var rssXml = XDocument.Parse(rssData);

                            // Phân tích và lưu dữ liệu RSS vào bảng Item
                            itemRepository.ParseAndSaveRss(rssXml, category.Id);

                            _logger.LogInformation($"Successfully fetched and saved data for category: {category.Name}");
                        }
                        catch (HttpRequestException e)
                        {
                            _logger.LogError($"Error fetching RSS feed for category {category.Name}: {e.Message}");
                        }
                    });

                    await Task.WhenAll(tasks); // Chờ tất cả các task trong batch hoàn thành
                }

            }
        }
    }
}
