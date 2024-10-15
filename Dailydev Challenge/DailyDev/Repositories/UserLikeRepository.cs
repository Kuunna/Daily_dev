using DailyDev.Models;
using System.Data.SqlClient;

namespace DailyDev.Repositories
{
    public class UserItemRepository
    {
        private readonly string _connectionString;

        public UserItemRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(UserItem userItem)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("INSERT INTO UserItem (UserId, ItemId) VALUES (@UserId, @ItemId)", connection);
                command.Parameters.AddWithValue("@UserId", userItem.UserId);
                command.Parameters.AddWithValue("@ItemId", userItem.ItemId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int userId, int itemId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM UserItem WHERE UserId = @UserId AND ItemId = @ItemId", connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@ItemId", itemId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

}
