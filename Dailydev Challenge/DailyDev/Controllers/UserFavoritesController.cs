using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using DailyDev.Models;

namespace DailyDev.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserFavoritesController : ControllerBase
    {
        private readonly string _connectionString;

        public UserFavoritesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // GET: api/userfavorites
        [HttpGet]
        public List<UserFavorite> GetUserFavorites()
        {
            var userFavorites = new List<UserFavorite>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT Id, UserId, SourceId FROM User_Favorites", connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    userFavorites.Add(new UserFavorite
                    {
                        Id = reader.GetInt32(0),
                        UserId = reader.GetInt32(1),
                        SourceId = reader.GetInt32(2)
                    });
                }
            }

            return userFavorites;
        }

        // POST: api/userfavorites
        [HttpPost]
        public IActionResult AddUserFavorite([FromBody] UserFavorite userFavorite)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO User_Favorites (UserId, SourceId) VALUES (@UserId, @SourceId)",
                    connection
                );

                command.Parameters.AddWithValue("@UserId", userFavorite.UserId);
                command.Parameters.AddWithValue("@SourceId", userFavorite.SourceId);

                command.ExecuteNonQuery();
            }

            return Ok("User favorite added successfully");
        }

        // DELETE: api/userfavorites/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUserFavorite(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM User_Favorites WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    return NotFound($"User favorite with ID {id} not found.");
                }
            }

            return Ok("User favorite deleted successfully.");
        }
    }
}
