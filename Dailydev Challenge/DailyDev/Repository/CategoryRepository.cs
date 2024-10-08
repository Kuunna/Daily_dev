using DailyDev.Models;
using HtmlAgilityPack;
using System.Data.SqlClient;

namespace DailyDev.Repository
{
    public class CategoryRepository
    {
        private readonly string _connectionString;

        public CategoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void Upsert(Category category)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // Kiểm tra xem mục nhập đã tồn tại hay chưa
                var checkCommand = new SqlCommand("SELECT COUNT(*) FROM Category WHERE Name = @Name", connection);
                checkCommand.Parameters.AddWithValue("@Name", category.Name);

                connection.Open();
                var count = (int)checkCommand.ExecuteScalar(); // ExecuteScalar() trả về giá trị đầu tiên của cột đầu tiên trong kết quả

                if (count > 0)
                {
                    Update(category);
                }
                else
                {
                    Add(category);
                }
            }
        }
        public void Add(Category category)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("INSERT INTO Category (Name, ProviderId, Source, ttl, generator, docs) VALUES (@Name, @ProviderId, @Source, @ttl, @generator, @docs)", connection);
                command.Parameters.AddWithValue("@Name", category.Name);
                command.Parameters.AddWithValue("@ProviderId", category.ProviderId);
                command.Parameters.AddWithValue("@Source", category.Source);
                command.Parameters.AddWithValue("@ttl", category.Ttl);
                command.Parameters.AddWithValue("@generator", category.Generator);
                command.Parameters.AddWithValue("@docs", category.Docs);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Category> GetAll()
        {
            var categories = new List<Category>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Category", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            ProviderId = (int)reader["ProviderId"],
                            Source = reader["Source"].ToString(),
                            Ttl = (int)reader["ttl"],
                            Generator = reader["generator"].ToString(),
                            Docs = reader["docs"].ToString()
                        });
                    }
                }
            }
            return categories;
        }

        public Category GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Category WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Category
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            ProviderId = (int)reader["ProviderId"],
                            Source = reader["Source"].ToString(),
                            Ttl = (int)reader["ttl"],
                            Generator = reader["generator"].ToString(),
                            Docs = reader["docs"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public void Update(Category category)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UPDATE Category SET Name = @Name, ProviderId = @ProviderId, Source = @Source, ttl = @ttl, generator = @generator, docs = @docs WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", category.Id);
                command.Parameters.AddWithValue("@Name", category.Name);
                command.Parameters.AddWithValue("@ProviderId", category.ProviderId);
                command.Parameters.AddWithValue("@Source", category.Source);
                command.Parameters.AddWithValue("@ttl", category.Ttl);
                command.Parameters.AddWithValue("@generator", category.Generator);
                command.Parameters.AddWithValue("@docs", category.Docs);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM Category WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void AddCategoryFromProvider(Provider provider)
        {
            // Tải nội dung RSS từ nguồn
            HtmlWeb web = new HtmlWeb();
            var doc = web.Load(provider.Source);

            if (provider.Source.Contains("vnexpress"))
            {
                // // Tìm các thẻ <ul> chứa danh sách các RSS feeds
                var rssNodes = doc.DocumentNode.SelectNodes("//ul[@class='list-rss']/li/a");
                if (rssNodes != null) // Kiểm tra nếu có thẻ <item>
                {
                    foreach (var node in rssNodes)
                    {
                        string name = node.SelectSingleNode("title")?.InnerText.Trim();
                        string source = node.SelectSingleNode("link")?.InnerText.Trim();

                        // Kiểm tra giá trị không null hoặc rỗng
                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(source))
                        {
                            var category = new Category
                            {
                                Name = name,
                                Source = source,
                                ProviderId = provider.Id
                            };
                            Upsert(category); 
                        }
                    }
                }
            }
            else if (provider.Source.Contains("tuoitre"))
            {
                // Tìm các thẻ <ul> chứa danh sách các RSS feeds
                var rssNodes = doc.DocumentNode.SelectNodes("//ul[@class='list-rss clearfix']/li/a");

                foreach (var node in rssNodes)
                {
                    string name = node.SelectSingleNode("title")?.InnerText.Trim();
                    string source = node.Attributes["href"].Value; // URL RSS

                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(source))
                    {
                        var category = new Category
                        {
                            Name = name,
                            Source = source,
                            ProviderId = provider.Id
                        };
                        Upsert(category);
                    }
                }
            }
            else if (provider.Source.Contains("dantri"))
            {
                // Tìm các thẻ <ul> chứa danh sách các RSS feeds
                var rssNodes = doc.DocumentNode.SelectNodes("//ol[@class='flex-col']/li/a");

                foreach (var node in rssNodes)
                {
                    string name = node.InnerText.Trim();
                    string source = node.Attributes["href"].Value; // URL RSS

                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(source))
                    {
                        var category = new Category
                        {
                            Name = name,
                            Source = source,
                            ProviderId = provider.Id
                        };
                        Upsert(category);
                    }
                }
            }
            else if (provider.Source.Contains("vietnamnet"))
            {
                
            }
            else if (provider.Source.Contains("laodong"))
            {
                // Tìm các thẻ <ul> chứa danh sách các RSS feeds
                var rssNodes = doc.DocumentNode.SelectNodes("//ul[@class='rss-lst']/li/a");

                foreach (var node in rssNodes)
                {
                    string name = node.InnerText.Trim();
                    string source = node.Attributes["href"].Value; // URL RSS

                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(source))
                    {
                        var category = new Category
                        {
                            Name = name,
                            Source = source,
                            ProviderId = provider.Id
                        };
                        Upsert(category);
                    }
                }
            }
            else if (provider.Source.Contains("nhandan"))
            {
                
            }
        }
    }
}
