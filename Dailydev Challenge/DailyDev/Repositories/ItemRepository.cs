using DailyDev.Models;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace DailyDev.Repository
{
    public class ItemRepository
    {
        private readonly string _connectionString;

        public ItemRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void Add(Item item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(@"
                    INSERT INTO Item (Title, Link, Guid, PubDate, Image, CategoryId, Author, Summary, Comments) 
                    VALUES (@Title, @Link, @Guid, @PubDate, @Image, @CategoryId, @Author, @Summary, @Comments)", connection);
                command.Parameters.AddWithValue("@Title", item.Title);
                command.Parameters.AddWithValue("@Link", item.Link);
                command.Parameters.AddWithValue("@Guid", item.Guid);
                command.Parameters.AddWithValue("@PubDate", item.PubDate);
                command.Parameters.AddWithValue("@Image", item.Image);
                command.Parameters.AddWithValue("@CategoryId", item.CategoryId);
                command.Parameters.AddWithValue("@Author", item.Author);
                command.Parameters.AddWithValue("@Summary", item.Summary);
                command.Parameters.AddWithValue("@Comments", item.Comments);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Item> GetAll()
        {
            var items = new List<Item>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Item", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new Item
                        {
                            Id = (int)reader["Id"],
                            Title = reader["Title"].ToString(),
                            Link = reader["Link"].ToString(),
                            Guid = reader["Guid"].ToString(),
                            PubDate = (DateTime)reader["PubDate"],
                            Image = reader["Image"].ToString(),
                            CategoryId = (int)reader["CategoryId"],
                            Author = reader["author"].ToString(),
                            Summary = reader["summary"].ToString(),
                            Comments = reader["comments"].ToString()
                        });
                    }
                }
            }
            return items;
        }

        public Item GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Item WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Item
                        {
                            Id = (int)reader["Id"],
                            Title = reader["Title"].ToString(),
                            Link = reader["Link"].ToString(),
                            Guid = reader["Guid"].ToString(),
                            PubDate = (DateTime)reader["PubDate"],
                            Image = reader["Image"].ToString(),
                            CategoryId = (int)reader["CategoryId"],
                            Author = reader["author"].ToString(),
                            Summary = reader["summary"].ToString(),
                            Comments = reader["comments"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public void Update(Item item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(@"
                    UPDATE Item 
                    SET 
                        Title = @Title, 
                        Link = @Link, 
                        Guid = @Guid, 
                        PubDate = @PubDate, 
                        Image = @Image, 
                        CategoryId = @CategoryId, 
                        author = @Author, 
                        summary = @Summary, 
                        comments = @Comments 
                    WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", item.Id);
                command.Parameters.AddWithValue("@Title", item.Title);
                command.Parameters.AddWithValue("@Link", item.Link);
                command.Parameters.AddWithValue("@Guid", item.Guid);
                command.Parameters.AddWithValue("@PubDate", item.PubDate);
                command.Parameters.AddWithValue("@Image", item.Image);
                command.Parameters.AddWithValue("@CategoryId", item.CategoryId);
                command.Parameters.AddWithValue("@Author", item.Author);
                command.Parameters.AddWithValue("@Summary", item.Summary);
                command.Parameters.AddWithValue("@Comments", item.Comments);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM Item WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Upsert(Item item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                /* var checkCommand = new SqlCommand("SELECT COUNT(*) FROM Item WHERE Guid = @Guid", connection);
                 checkCommand.Parameters.AddWithValue("@Guid", item.Guid);

                 connection.Open();
                 var count = (int)checkCommand.ExecuteScalar(); */
                if (Exists(item.Guid))
                {
                    Update(item);
                }
                else
                {
                    Add(item);
                }
            }
        }

        // Kiểm tra nếu một bài viết đã tồn tại dựa trên Guid
        public bool Exists(string guid)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // Kiểm tra xem mục nhập đã tồn tại hay chưa
                var command = new SqlCommand("SELECT COUNT(*) FROM Item WHERE Guid = @Guid", connection);
                command.Parameters.AddWithValue("@Guid", guid);

                connection.Open();
                // ExecuteScalar() trả về giá trị đầu tiên của cột đầu tiên trong kết quả
                var count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }


        // Hàm phân tích và lưu RSS
        public void ParseAndSaveRss(XDocument rssXml, int categoryId)
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

                // Lấy img từ thẻ <enclosure>
                var enclosure = item.Element("enclosure");
                var imageUrl = enclosure?.Attribute("url")?.Value;

                // Chuyển đổi pubDate thành DateTime
                var parsedDate = ParseRssDate(pubDate);

                // Tạo đối tượng Item từ dữ liệu RSS
                var newItem = new Item
                {
                    Title = title,
                    Link = link,
                    Guid = guid,
                    PubDate = parsedDate,
                    Image = imageUrl ?? "Null",
                    CategoryId = categoryId, 
                    Author = author ?? "Null",
                    Summary = summary ?? "Null",
                    Comments = comments ?? "Null"
                };

                // Lưu vào cơ sở dữ liệu
                Upsert(newItem);
            }
        }
        public DateTime ParseRssDate(string dateString)
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
