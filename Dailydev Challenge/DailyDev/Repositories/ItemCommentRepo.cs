using DailyDev.Models;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace DailyDev.Repositories
{
    public class ItemCommentRepo
    {
        private readonly string _connectionString;

        public ItemCommentRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task Add(ItemComment comment)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "INSERT INTO ItemComment (UserId, ItemId, Content, ParentId, CreateAt) " +
                            "VALUES (@UserId, @ItemId, @Content, @ParentId, @CreateAt)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", comment.UserId);
                    cmd.Parameters.AddWithValue("@ItemId", comment.ItemId);
                    cmd.Parameters.AddWithValue("@Content", comment.Content);
                    cmd.Parameters.AddWithValue("@ParentId", (object)comment.ParentId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreateAt", comment.CreateAt);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<ItemComment>> GetByItemId(int itemId)
        {
            var comments = new List<ItemComment>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT * FROM ItemComment WHERE ItemId = @ItemId ORDER BY CreateAt DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ItemId", itemId);
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            comments.Add(new ItemComment
                            {
                                Id = reader.GetInt32(0),
                                UserId = reader.GetInt32(1),
                                ItemId = reader.GetInt32(2),
                                Content = reader.GetString(3),
                                ParentId = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                                CreateAt = reader.GetDateTime(5)
                            });
                        }
                    }
                }
            }
            return comments;
        }

        public async Task<ItemComment> GetById(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT * FROM ItemComment WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new ItemComment
                            {
                                Id = reader.GetInt32(0),
                                UserId = reader.GetInt32(1),
                                ItemId = reader.GetInt32(2),
                                Content = reader.GetString(3),
                                ParentId = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                                CreateAt = reader.GetDateTime(5)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public async Task Update(ItemComment comment)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "UPDATE ItemComment SET Content = @Content WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Content", comment.Content);
                    cmd.Parameters.AddWithValue("@Id", comment.Id);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var query = "DELETE FROM ItemComment WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
