using DailyDev.Models;
using System.Data.SqlClient;

namespace DailyDev.Repositories
{
    public class UserItemRepo
    {
        private readonly string _connectionString;

        public UserItemRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void LikeItem(int userId, int itemId, bool isLiked)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(@"
                IF EXISTS (SELECT 1 FROM UserItem WHERE UserId = @UserId AND ItemId = @ItemId)
                BEGIN
                    UPDATE UserItem SET IsLiked = @IsLiked, LikeDate = CASE WHEN @IsLiked = 1 THEN GETDATE() ELSE NULL END
                    WHERE UserId = @UserId AND ItemId = @ItemId;
                END
                ELSE
                BEGIN
                    INSERT INTO UserItem (UserId, ItemId, IsLiked, LikeDate) 
                    VALUES (@UserId, @ItemId, @IsLiked, CASE WHEN @IsLiked = 1 THEN GETDATE() ELSE NULL END);
                END
            ", connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@ItemId", itemId);
                command.Parameters.AddWithValue("@IsLiked", isLiked);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UnlikeItem(int userId, int itemId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(@"
                UPDATE UserItem SET IsLiked = 0, LikeDate = NULL 
                WHERE UserId = @UserId AND ItemId = @ItemId;
            ", connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@ItemId", itemId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void BookmarkItem(int userId, int itemId, bool isBookmarked)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(@"
                IF EXISTS (SELECT 1 FROM UserItem WHERE UserId = @UserId AND ItemId = @ItemId)
                BEGIN
                    UPDATE UserItem SET IsBookmarked = @IsBookmarked, BookmarkDate = CASE WHEN @IsBookmarked = 1 THEN GETDATE() ELSE NULL END
                    WHERE UserId = @UserId AND ItemId = @ItemId;
                END
                ELSE
                BEGIN
                    INSERT INTO UserItem (UserId, ItemId, IsBookmarked, BookmarkDate) 
                    VALUES (@UserId, @ItemId, @IsBookmarked, CASE WHEN @IsBookmarked = 1 THEN GETDATE() ELSE NULL END);
                END
            ", connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@ItemId", itemId);
                command.Parameters.AddWithValue("@IsBookmarked", isBookmarked);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UnbookmarkItem(int userId, int itemId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(@"
                UPDATE UserItem SET IsBookmarked = 0, BookmarkDate = NULL 
                WHERE UserId = @UserId AND ItemId = @ItemId;
            ", connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@ItemId", itemId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
