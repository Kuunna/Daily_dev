using DailyDev.Models;
using System.Data.SqlClient;

namespace DailyDev.Repository
{
    public class UserTagRepository
    {
        private readonly string _connectionString;

        public UserTagRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(UserTag userTag)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("INSERT INTO UserTag (UserId, TagId) VALUES (@UserId, @TagId)", connection);
                command.Parameters.AddWithValue("@UserId", userTag.UserId);
                command.Parameters.AddWithValue("@TagId", userTag.TagId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<UserTag> GetAll()
        {
            var userTags = new List<UserTag>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM UserTag", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userTags.Add(new UserTag
                        {
                            Id = (int)reader["Id"],
                            UserId = (int)reader["UserId"],
                            TagId = (int)reader["TagId"]
                        });
                    }
                }
            }
            return userTags;
        }

        // Các phương thức Update và Delete ở đây.
    }

}
