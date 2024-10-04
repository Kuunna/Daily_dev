using DailyDev.Models;
using System.Data.SqlClient;

namespace DailyDev.Repository
{
    public class CategoryRepository
    {
        private readonly string _connectionString;

        public CategoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Category category)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("INSERT INTO Category (Name, ProviderId, Source, ttl, generator, docs) VALUES (@Name, @ProviderId, @Source, @ttl, @generator, @docs)", connection);
                command.Parameters.AddWithValue("@Name", category.Name);
                command.Parameters.AddWithValue("@ProviderId", category.ProviderId);
                command.Parameters.AddWithValue("@Source", category.Source);
                command.Parameters.AddWithValue("@ttl", category.Ttl);
                command.Parameters.AddWithValue("@generator", category.Generator);
                command.Parameters.AddWithValue("@docs", category.Docs);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Category> GetAll()
        {
            var categories = new List<Category>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Category", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            ProviderId = (int)reader["ProviderId"],
                            Source = reader["Source"].ToString(),
                            Ttl = (int)reader["ttl"],
                            Generator = reader["generator"].ToString(),
                            Docs = reader["docs"].ToString()
                        });
                    }
                }
            }
            return categories;
        }

        public Category GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Category WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Category
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            ProviderId = (int)reader["ProviderId"],
                            Source = reader["Source"].ToString(),
                            Ttl = (int)reader["ttl"],
                            Generator = reader["generator"].ToString(),
                            Docs = reader["docs"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public void Update(Category category)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UPDATE Category SET Name = @Name, ProviderId = @ProviderId, Source = @Source, ttl = @ttl, generator = @generator, docs = @docs WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", category.Id);
                command.Parameters.AddWithValue("@Name", category.Name);
                command.Parameters.AddWithValue("@ProviderId", category.ProviderId);
                command.Parameters.AddWithValue("@Source", category.Source);
                command.Parameters.AddWithValue("@ttl", category.Ttl);
                command.Parameters.AddWithValue("@generator", category.Generator);
                command.Parameters.AddWithValue("@docs", category.Docs);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM Category WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }


}
