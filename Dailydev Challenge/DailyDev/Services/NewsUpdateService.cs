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
        private readonly ItemRepository _itemRepository;
        private readonly CategoryRepository _categoryRepository;

        public NewsUpdateService(HttpClient httpClient, ItemRepository itemRepository, CategoryRepository categoryRepository)
        {
            _httpClient = httpClient;
            _itemRepository = itemRepository;
            _categoryRepository = categoryRepository;
        }

        // Hàm chạy background service
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Gọi hàm để cập nhật tin tức
                    await FetchAndUpdateNews();
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi nếu cần
                    Console.WriteLine($"Error updating news: {ex.Message}");
                }

                // Đợi 24 giờ (86400000 milliseconds)
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        // Hàm cập nhật tin tức
        private async Task FetchAndUpdateNews()
        {
            // Lấy tất cả các Category từ bảng Category
            var categories = _categoryRepository.GetAll();

            foreach (var category in categories)
            {
                var response = await _httpClient.GetAsync(category.Source);
                response.EnsureSuccessStatusCode();

                var rssData = await response.Content.ReadAsStringAsync();
                var rssXml = XDocument.Parse(rssData);

                // Chỉ lấy các bài viết mới
                _itemRepository.ParseAndSaveRss(rssXml, category.Id);
            }
        }
    }
}
