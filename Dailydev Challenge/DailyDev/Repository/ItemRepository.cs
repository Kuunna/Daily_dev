using DailyDev.Models;
using System.Data.SqlClient;

namespace DailyDev.Repository
{
    public class ItemRepository
    {
        private readonly string _connectionString;

        public ItemRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void Upsert(Item item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // Kiểm tra xem mục nhập đã tồn tại hay chưa
                var checkCommand = new SqlCommand("SELECT COUNT(*) FROM Item WHERE Guid = @Guid", connection);
                checkCommand.Parameters.AddWithValue("@Guid", item.Guid);

                connection.Open();
                var count = (int)checkCommand.ExecuteScalar(); // ExecuteScalar() trả về giá trị đầu tiên của cột đầu tiên trong kết quả

                if (count > 0)
                {
                    Update(item);
                }
                else
                {
                    Add(item);
                }
            }
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

    }

}
