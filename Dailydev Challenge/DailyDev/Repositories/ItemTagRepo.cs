using DailyDev.Models;
using System.Data.SqlClient;

namespace DailyDev.Repository
{
    public class ItemTagRepo
    {
        private readonly string _connectionString;

        public ItemTagRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(ItemTag itemTag)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("INSERT INTO ItemTag (ItemId, TagId) VALUES (@ItemId, @TagId)", connection);
                command.Parameters.AddWithValue("@ItemId", itemTag.ItemId);
                command.Parameters.AddWithValue("@TagId", itemTag.TagId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<ItemTag> GetAll()
        {
            var itemTags = new List<ItemTag>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM ItemTag", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        itemTags.Add(new ItemTag
                        {
                            Id = (int)reader["Id"],
                            ItemId = (int)reader["ItemId"],
                            TagId = (int)reader["TagId"]
                        });
                    }
                }
            }
            return itemTags;
        }

        public ItemTag GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM ItemTag WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ItemTag
                        {
                            Id = (int)reader["Id"],
                            ItemId = (int)reader["ItemId"],
                            TagId = (int)reader["TagId"]
                        };
                    }
                }
            }
            return null;
        }

        public void Update(ItemTag itemTag)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UPDATE ItemTag SET ItemId = @ItemId, TagId = @TagId WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", itemTag.Id);
                command.Parameters.AddWithValue("@ItemId", itemTag.ItemId);
                command.Parameters.AddWithValue("@TagId", itemTag.TagId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM ItemTag WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }


}
