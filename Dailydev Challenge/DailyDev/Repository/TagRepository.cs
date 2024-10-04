using DailyDev.Models;
using System.Data.SqlClient;

namespace DailyDev.Repository
{
    public class TagRepository
    {
        private readonly string _connectionString;

        public TagRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Tag tag)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("INSERT INTO Tag (Name, description) VALUES (@Name, @Description)", connection);
                command.Parameters.AddWithValue("@Name", tag.Name);
                command.Parameters.AddWithValue("@Description", tag.Description);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Tag> GetAll()
        {
            var tags = new List<Tag>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Tag", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tags.Add(new Tag
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Description = reader["description"].ToString()
                        });
                    }
                }
            }
            return tags;
        }

        public Tag GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Tag WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Tag
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Description = reader["description"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public void Update(Tag tag)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UPDATE Tag SET Name = @Name, description = @Description WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", tag.Id);
                command.Parameters.AddWithValue("@Name", tag.Name);
                command.Parameters.AddWithValue("@Description", tag.Description);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM Tag WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

}
