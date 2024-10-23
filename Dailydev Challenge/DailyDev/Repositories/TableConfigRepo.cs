using DailyDev.Models;
using System.Data.SqlClient;

namespace DailyDev.Repositories
{
    public class TableConfigRepo
    {
        private readonly string _connectionString;

        public TableConfigRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(TableConfig tableConfig)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("INSERT INTO TableConfig (UserId, MostLiked, MostRead, MostTagged, FavoriteCategory) VALUES (@UserId, @MostLiked, @MostRead, @MostTagged, @FavoriteCategory)", connection);
                command.Parameters.AddWithValue("@UserId", tableConfig.UserId);
                command.Parameters.AddWithValue("@MostLiked", tableConfig.MostLiked);
                command.Parameters.AddWithValue("@MostRead", tableConfig.MostRead);
                command.Parameters.AddWithValue("@MostTagged", tableConfig.MostTagged);
                command.Parameters.AddWithValue("@FavoriteCategory", tableConfig.FavoriteCategory);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<TableConfig> GetAll()
        {
            var tableConfigs = new List<TableConfig>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM TableConfig", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tableConfigs.Add(new TableConfig
                        {
                            Id = (int)reader["Id"],
                            UserId = (int)reader["UserId"],
                            MostLiked = (int)reader["MostLiked"],
                            MostRead = (int)reader["MostRead"],
                            MostTagged = (int)reader["MostTagged"],
                            FavoriteCategory = (int)reader["FavoriteCategory"]
                        });
                    }
                }
            }
            return tableConfigs;
        }

        public void Update(TableConfig tableConfig)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UPDATE TableConfig SET UserId = @UserId, MostLiked = @MostLiked, MostRead = @MostRead, MostTagged = @MostTagged, FavoriteCategory = @FavoriteCategory WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", tableConfig.Id);
                command.Parameters.AddWithValue("@UserId", tableConfig.UserId);
                command.Parameters.AddWithValue("@MostLiked", tableConfig.MostLiked);
                command.Parameters.AddWithValue("@MostRead", tableConfig.MostRead);
                command.Parameters.AddWithValue("@MostTagged", tableConfig.MostTagged);
                command.Parameters.AddWithValue("@FavoriteCategory", tableConfig.FavoriteCategory);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM TableConfig WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

}
