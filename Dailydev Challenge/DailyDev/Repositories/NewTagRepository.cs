using DailyDev.Models;
using System.Data.SqlClient;

namespace DailyDev.Repository
{
    public class NewTagRepository
    {
        private readonly string _connectionString;

        public NewTagRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(NewTag newTag)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("INSERT INTO NewTag (NewId, TagId) VALUES (@NewId, @TagId)", connection);
                command.Parameters.AddWithValue("@NewId", newTag.NewId);
                command.Parameters.AddWithValue("@TagId", newTag.TagId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<NewTag> GetAll()
        {
            var newTags = new List<NewTag>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM NewTag", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        newTags.Add(new NewTag
                        {
                            Id = (int)reader["Id"],
                            NewId = (int)reader["NewId"],
                            TagId = (int)reader["TagId"]
                        });
                    }
                }
            }
            return newTags;
        }

        public NewTag GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM NewTag WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new NewTag
                        {
                            Id = (int)reader["Id"],
                            NewId = (int)reader["NewId"],
                            TagId = (int)reader["TagId"]
                        };
                    }
                }
            }
            return null;
        }

        public void Update(NewTag newTag)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UPDATE NewTag SET NewId = @NewId, TagId = @TagId WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", newTag.Id);
                command.Parameters.AddWithValue("@NewId", newTag.NewId);
                command.Parameters.AddWithValue("@TagId", newTag.TagId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM NewTag WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }


}
