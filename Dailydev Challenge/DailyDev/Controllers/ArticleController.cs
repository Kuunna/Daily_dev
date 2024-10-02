using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using DailyDev.Models;

namespace DailyDev.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly string _connectionString;

        public ArticlesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // GET: api/articles
        [HttpGet]
        public List<Article> GetArticles()
        {
            var articles = new List<Article>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT Id, Title, Description, Url, PublishedDate, SourceId FROM Articles", connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    articles.Add(new Article
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.GetString(2),
                        Url = reader.GetString(3),
                        PublishedDate = reader.GetDateTime(4),
                        SourceId = reader.GetInt32(5)
                    });
                }
            }

            return articles;
        }

        // POST: api/articles
        [HttpPost]
        public IActionResult AddArticle([FromBody] Article article)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO Articles (Title, Description, Url, PublishedDate, SourceId) VALUES (@Title, @Description, @Url, @PublishedDate, @SourceId)",
                    connection
                );

                command.Parameters.AddWithValue("@Title", article.Title);
                command.Parameters.AddWithValue("@Description", article.Description);
                command.Parameters.AddWithValue("@Url", article.Url);
                command.Parameters.AddWithValue("@PublishedDate", article.PublishedDate);
                command.Parameters.AddWithValue("@SourceId", article.SourceId);

                command.ExecuteNonQuery();
            }

            return Ok("Article added successfully");
        }

        // PUT: api/articles/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateArticle(int id, [FromBody] Article article)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "UPDATE Articles SET Title = @Title, Description = @Description, Url = @Url, PublishedDate = @PublishedDate, SourceId = @SourceId WHERE Id = @Id",
                    connection
                );

                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Title", article.Title);
                command.Parameters.AddWithValue("@Description", article.Description);
                command.Parameters.AddWithValue("@Url", article.Url);
                command.Parameters.AddWithValue("@PublishedDate", article.PublishedDate);
                command.Parameters.AddWithValue("@SourceId", article.SourceId);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    return NotFound($"Article with ID {id} not found.");
                }
            }

            return Ok("Article updated successfully.");
        }

        // DELETE: api/articles/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteArticle(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Articles WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    return NotFound($"Article with ID {id} not found.");
                }
            }

            return Ok("Article deleted successfully.");
        }
    }
}
