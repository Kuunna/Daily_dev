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

        public void Upsert(UserTag userTag)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // Kiểm tra xem mục nhập đã tồn tại hay chưa
                var checkCommand = new SqlCommand("SELECT COUNT(*) FROM userTag WHERE UserId = @UserId AND TagId = @TagId", connection);
                checkCommand.Parameters.AddWithValue("@UserId", userTag.UserId);
                checkCommand.Parameters.AddWithValue("@TagId", userTag.TagId);

                connection.Open();
                var count = (int)checkCommand.ExecuteScalar(); // ExecuteScalar() trả về giá trị đầu tiên của cột đầu tiên trong kết quả

                if (count > 0)
                {
                    Update(userTag);
                }
                else
                {
                    Add(userTag);
                }
            }
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


        public void Update(UserTag userTag)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UPDATE UserTag SET UserId = @UserId, TagId = @TagId WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", userTag.Id);
                command.Parameters.AddWithValue("@UserId", userTag.UserId);
                command.Parameters.AddWithValue("@TagId", userTag.TagId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM UserTag WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
