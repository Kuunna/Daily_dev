﻿using DailyDev.Models;
using DailyDev.Repositories;
using System.Xml.Linq;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DailyDev.Service
{
    public class UpdateService : BackgroundService
    {
        private readonly HttpClient _httpClient;
        private readonly IServiceProvider _serviceProvider; // Thêm IServiceProvider
        private readonly ILogger<UpdateService> _logger;

        public UpdateService(
            HttpClient httpClient,
            IServiceProvider serviceProvider,
            ILogger<UpdateService> logger)
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
                    _logger.LogInformation("Starting category update...");

                    await FetchAndUpdateCategories(stoppingToken); // Cập nhật categories

                    _logger.LogInformation("Category update completed.");


                    _logger.LogInformation("Starting RSS feed update...");

                    await FetchAndUpdateNews(stoppingToken); // Cập nhật RSS feeds

                    _logger.LogInformation("RSS feed update completed.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating categories or RSS feeds.");
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        // Take categories from sources in table Provider
        private async Task FetchAndUpdateCategories(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope()) 
            {
                var categoryRepository = scope.ServiceProvider.GetRequiredService<CategoryRepo>();
                var providerRepository = scope.ServiceProvider.GetRequiredService<ProviderRepo>();

                var providers = providerRepository.GetAll();
                int batchSize = 2; 
                _httpClient.Timeout = TimeSpan.FromMinutes(5);

                for (int i = 0; i < providers.Count(); i += batchSize)
                {
                    var batchProviders = providers.Skip(i).Take(batchSize);
                    var tasks = batchProviders.Select(async provider =>
                    {
                        _logger.LogInformation($"Fetching categories from provider: {provider.Name}");

                        try
                        {
                            categoryRepository.AddCategoryFromProvider(provider);
                            _logger.LogInformation($"Successfully fetched and updated categories for provider: {provider.Name}");
                        }
                        catch (Exception e)
                        {
                            _logger.LogError($"Error fetching categories for provider {provider.Name}: {e.Message}");
                        }
                    });

                    await Task.WhenAll(tasks);
                }
            }
        }


        // Hàm lấy RSS từ các category và lưu vào database
        private async Task FetchAndUpdateNews(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var itemRepository = scope.ServiceProvider.GetRequiredService<ItemRepo>();
                var categoryRepository = scope.ServiceProvider.GetRequiredService<CategoryRepo>();

                var categories = categoryRepository.GetAll();
                int batchSize = 5; 
                _httpClient.Timeout = TimeSpan.FromMinutes(5);

                for (int i = 0; i < categories.Count(); i += batchSize)
                {
                    var batchCategories = categories.Skip(i).Take(batchSize);
                    var tasks = batchCategories.Select(async category =>
                    {
                        _logger.LogInformation($"Fetching RSS feed for category: {category.Name}");

                        try
                        {
                            var response = await _httpClient.GetAsync(category.Source, cancellationToken);
                            response.EnsureSuccessStatusCode();

                            var rssData = await response.Content.ReadAsStringAsync();
                            var rssXml = XDocument.Parse(rssData);

                            itemRepository.ParseAndSaveRss(rssXml, category.Id);

                            _logger.LogInformation($"Successfully fetched and saved data for category: {category.Name}");
                        }
                        catch (HttpRequestException e)
                        {
                            _logger.LogError($"Error fetching RSS feed for category {category.Name}: {e.Message}");
                        }
                    });

                    await Task.WhenAll(tasks); 
                }

            }
        }
    }
}
