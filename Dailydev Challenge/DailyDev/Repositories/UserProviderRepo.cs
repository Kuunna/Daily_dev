using DailyDev.Models;
using System.Data.SqlClient;

namespace DailyDev.Repositories
{
    public class UserProviderRepo
    {
        private readonly string _connectionString;

        public UserProviderRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(UserProvider userProvider)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("INSERT INTO UserProvider (UserId, ProviderId) VALUES (@UserId, @ProviderId)", connection);
                command.Parameters.AddWithValue("@UserId", userProvider.UserId);
                command.Parameters.AddWithValue("@ProviderId", userProvider.ProviderId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<UserProvider> GetAll()
        {
            var userProviders = new List<UserProvider>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM UserProvider", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userProviders.Add(new UserProvider
                        {
                            Id = (int)reader["Id"],
                            UserId = (int)reader["UserId"],
                            ProviderId = (int)reader["ProviderId"]
                        });
                    }
                }
            }
            return userProviders;
        }

        public UserProvider GetByUserId(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM [UserProvider] WHERE UserId = @UserId", connection);
                command.Parameters.Add(new SqlParameter("@UserId", userId));
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UserProvider
                        {
                            Id = (int)reader["Id"],
                            UserId = (int)reader["UserId"],
                            ProviderId = (int)reader["ProviderId"]
                        };
                    }
                }
            }
            return null;
        }


        public void Update(UserProvider userProvider)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UPDATE UserProvider SET UserId = @UserId, ProviderId = @ProviderId WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", userProvider.Id);
                command.Parameters.AddWithValue("@UserId", userProvider.UserId);
                command.Parameters.AddWithValue("@ProviderId", userProvider.ProviderId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int userId, int providerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM UserProvider WHERE UserId = @UserId AND ProviderId = @ProviderId", connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@ProviderId", providerId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

}
