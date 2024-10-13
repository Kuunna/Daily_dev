using DailyDev.Models;
using System.Data.SqlClient;

namespace DailyDev.Repositories
{
    public class UserLikeRepository
    {
        private readonly string _connectionString;

        public UserLikeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(UserLike userLike)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("INSERT INTO UserLike (UserId, ItemId) VALUES (@UserId, @ItemId)", connection);
                command.Parameters.AddWithValue("@UserId", userLike.UserId);
                command.Parameters.AddWithValue("@ItemId", userLike.ItemId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int userId, int itemId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM UserLike WHERE UserId = @UserId AND ItemId = @ItemId", connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@ItemId", itemId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

}
