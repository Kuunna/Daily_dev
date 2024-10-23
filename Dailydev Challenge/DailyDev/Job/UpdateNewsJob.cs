using Quartz;
using DailyDev.Models;
using DailyDev.Repositories;

namespace DailyDev.Job
{
    public class UpdateNewsJob : IJob
    {
        private readonly ProviderRepo _providerRepo;
        private readonly IHttpClientFactory _httpClientFactory;

        public UpdateNewsJob(ProviderRepo providerRepo, IHttpClientFactory httpClientFactory)
        {
            _providerRepo = providerRepo;
            _httpClientFactory = httpClientFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var providers = _providerRepo.GetAll();

            foreach (var provider in providers)
            {
                if (provider.Status == "In Progress" || provider.Status == "Not Started")
                {
                    provider.Status = "In Progress";
                    provider.ProcessAt = DateTime.Now;

                    try
                    {
                        var httpClient = _httpClientFactory.CreateClient();
                        var response = await httpClient.GetStringAsync(provider.Source);s

                        await ParseAndSaveRss(response, provider);

                        provider.Status = "Processed";
                    }
                    catch (Exception ex)
                    {
                        provider.Status = "Failed";
                    }
                    finally
                    {
                        provider.ProcessAt = DateTime.Now;
                        _providerRepo.Update(provider);
                    }
                }
            }
        }

        private async Task ParseAndSaveRss(string rssData, Provider provider)
        {
            // Cách xử lý dữ liệu RSS
            // Ví dụ: phân tích và lưu vào database
        }
    }
}
