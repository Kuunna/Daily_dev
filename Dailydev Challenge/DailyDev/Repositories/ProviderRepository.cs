using DailyDev.Models;
using System.Data.SqlClient;

namespace DailyDev.Repository
{
    public class ProviderRepository
    {
        private readonly string _connectionString;

        public ProviderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void Add(Provider provider)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("INSERT INTO Provider (Name, Source) VALUES (@Name, @Source)", connection);
                command.Parameters.AddWithValue("@Name", provider.Name);
                command.Parameters.AddWithValue("@Source", provider.Source);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Provider> GetAll()
        {
            var providers = new List<Provider>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Provider", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        providers.Add(new Provider
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Source = reader["Source"].ToString()
                        });
                    }
                }
            }
            return providers;
        }

        public Provider GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Provider WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Provider
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Source = reader["Source"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public void Update(Provider provider)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UPDATE Provider SET Name = @Name, Source = @Source WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", provider.Id);
                command.Parameters.AddWithValue("@Name", provider.Name);
                command.Parameters.AddWithValue("@Source", provider.Source);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM Provider WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
