using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using DailyDev.Models;

namespace DailyDev.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SourceController : ControllerBase
    {
        private readonly string _connectionString;

        // Constructor nhận chuỗi kết nối từ appsettings.json
        public SourceController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // GET: api/source - Lấy danh sách nguồn tin
        [HttpGet]
        public IActionResult GetSources()
        {
            var sources = new List<Source>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT Id, Name, Url, LastUpdated FROM Sources", connection);
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        sources.Add(new Source
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Url = reader.GetString(2),
                            LastUpdated = reader.GetDateTime(3)
                        });
                    }
                }
            }
            catch (SqlException ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }

            return Ok(sources);
        }


        // POST: api/source - Thêm nguồn tin mới
        [HttpPost]
        public IActionResult AddSource([FromBody] Source source)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Câu lệnh SQL để thêm nguồn tin vào bảng Sources
                var command = new SqlCommand(
                    "INSERT INTO Sources (Name, Url, LastUpdated) VALUES (@Name, @Url, @LastUpdated)",
                    connection
                );

                command.Parameters.AddWithValue("@Name", source.Name);
                command.Parameters.AddWithValue("@Url", source.Url);
                command.Parameters.AddWithValue("@LastUpdated", source.LastUpdated);

                // Thực thi câu lệnh
                command.ExecuteNonQuery();
            }

            return Ok("Source added successfully");
        }

        // PUT: api/source/{id} - Cập nhật nguồn tin
        [HttpPut("{id}")]
        public IActionResult UpdateSource(int id, [FromBody] Source source)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand(
                    "UPDATE Sources SET Name = @Name, Url = @Url, LastUpdated = @LastUpdated WHERE Id = @Id",
                    connection
                );

                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Name", source.Name);
                command.Parameters.AddWithValue("@Url", source.Url);
                command.Parameters.AddWithValue("@LastUpdated", source.LastUpdated);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return NotFound($"Source with ID {id} not found.");
                }
            }

            return Ok("Source updated successfully.");
        }

        // DELETE: api/source/{id} - Xóa nguồn tin
        [HttpDelete("{id}")]
        public IActionResult DeleteSource(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand("DELETE FROM Sources WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return NotFound($"Source with ID {id} not found.");
                }
            }

            return Ok("Source deleted successfully.");
        }


    }

}
