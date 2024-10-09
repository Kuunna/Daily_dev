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
                var checkCommand = new SqlCommand("SELECT COUNT(*) FROM Category WHERE Source = @Source", connection);
                checkCommand.Parameters.AddWithValue("@Source", category.Source);

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
                var command = new SqlCommand("INSERT INTO Category (Name, ProviderId, Source) VALUES (@Name, @ProviderId, @Source)", connection);
                command.Parameters.AddWithValue("@Name", category.Name);
                command.Parameters.AddWithValue("@ProviderId", category.ProviderId);
                command.Parameters.AddWithValue("@Source", category.Source);
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
                            Source = reader["Source"].ToString()
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
                            Source = reader["Source"].ToString()
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
                var command = new SqlCommand("UPDATE Category SET Name = @Name, ProviderId = @ProviderId, Source = @Source WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", category.Id);
                command.Parameters.AddWithValue("@Name", category.Name);
                command.Parameters.AddWithValue("@ProviderId", category.ProviderId);
                command.Parameters.AddWithValue("@Source", category.Source);
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
                // Tìm các thẻ <a> chứa các RSS feeds trong trang vnexpress.net/rss
                var rssNodes = doc.DocumentNode.SelectNodes("//div[@class='wrap-list-rss']/ul/li/a");

                if (rssNodes != null) // Kiểm tra nếu có các thẻ <a> chứa RSS feeds
                {
                    foreach (var node in rssNodes)
                    {
                        // Lấy tên category từ nội dung của thẻ <a> nhưng bỏ qua thẻ <span>
                        string name = node.SelectSingleNode("text()")?.InnerText.Trim();
                        // Lấy URL của RSS từ thuộc tính href của thẻ <a>
                        string source = "https://vnexpress.net" + node.Attributes["href"]?.Value.Trim();

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
                // Tìm các thẻ <a> chứa các RSS feeds trong trang vnexpress.net/rss
                var rssNodes = doc.DocumentNode.SelectNodes("//ul[@class='list-rss clearfix']/li/a");

                if (rssNodes != null) // Kiểm tra nếu có các thẻ <a> chứa RSS feeds
                {
                    foreach (var node in rssNodes)
                    {
                        // Lấy tên category từ nội dung của thẻ <a> nhưng bỏ qua thẻ <span>
                        string name = node.SelectSingleNode("text()")?.InnerText.Trim();
                        // Lấy URL của RSS từ thuộc tính href của thẻ <a>
                        string source = "https://tuoitre.vn" + node.Attributes["href"]?.Value.Trim();

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
            else if (provider.Source.Contains("dantri"))
            {
                // Tìm các thẻ <li> chứa RSS feeds
                var liNodes = doc.DocumentNode.SelectNodes("//ol[@class='flex-col']/li");
                if (liNodes != null) // Kiểm tra nếu có các thẻ <li>
                {
                    foreach (var liNode in liNodes)
                    {
                        // Lấy thẻ <a> đầu tiên trong mỗi <li>
                        var firstAnchor = liNode.SelectSingleNode("a");

                        if (firstAnchor != null)
                        {
                            // Lấy tên category từ nội dung của thẻ <a>, bỏ qua các thẻ con
                            string name = firstAnchor.SelectSingleNode("text()")?.InnerText.Trim();

                            // Lấy URL của RSS từ thuộc tính href của thẻ <a>
                            string source = "https://dantri.com.vn" + firstAnchor.Attributes["href"]?.Value.Trim();

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
            }
            else if (provider.Source.Contains("vietnamnet"))
            {
                // Tìm các thẻ <a> chứa các RSS feeds trong trang vnexpress.net/rss
                var rssNodes = doc.DocumentNode.SelectNodes("//div[@class='list']/div/div/a");

                if (rssNodes != null) // Kiểm tra nếu có các thẻ <a> chứa RSS feeds
                {
                    foreach (var node in rssNodes)
                    {
                        // Lấy tên category từ nội dung của thẻ <a> nhưng bỏ qua thẻ <span>
                        string name = node.SelectSingleNode("text()")?.InnerText.Trim();
                        // Lấy URL của RSS từ thuộc tính href của thẻ <a>
                        string source = node.Attributes["href"]?.Value.Trim();

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
            else if (provider.Source.Contains("laodong"))
            {
                // Tìm các thẻ <li> chứa RSS feeds
                var liNodes = doc.DocumentNode.SelectNodes("//ul[@class='rss-lst']/li");
                if (liNodes != null) // Kiểm tra nếu có các thẻ <li>
                {
                    foreach (var liNode in liNodes)
                    {
                        // Lấy thẻ <a> đầu tiên trong mỗi <li>
                        var firstAnchor = liNode.SelectSingleNode("a");

                        if (firstAnchor != null)
                        {
                            // Lấy tên category từ nội dung của thẻ <a>, bỏ qua các thẻ con
                            string name = firstAnchor.SelectSingleNode("text()")?.InnerText.Trim();

                            // Lấy URL của RSS từ thuộc tính href của thẻ <a>
                            string source = firstAnchor.Attributes["href"]?.Value.Trim();

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
            }
            else if (provider.Source.Contains("nhandan"))
            {
                // Tìm các thẻ <li> chứa RSS feeds
                var liNodes = doc.DocumentNode.SelectNodes("//ul[@class='rss-channels']/li");

                if (liNodes != null) // Kiểm tra nếu có các thẻ <li>
                {
                    foreach (var liNode in liNodes)
                    {
                        // Lấy thẻ <a> thứ hai trong mỗi <li>
                        var secondAnchor = liNode.SelectSingleNode("a[2]");

                        if (secondAnchor != null)
                        {
                            // Lấy tên category từ nội dung của thẻ <a> thứ hai, bỏ qua các thẻ con
                            string name = secondAnchor.SelectSingleNode("text()")?.InnerText.Trim();

                            // Lấy URL của RSS từ thuộc tính href của thẻ <a> thứ hai
                            string source = "https://nhandan.vn" + secondAnchor.Attributes["href"]?.Value.Trim();

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
            }

        }
    }
}
