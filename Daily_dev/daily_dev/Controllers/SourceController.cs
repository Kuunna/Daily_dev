using daily_dev.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ServiceModel.Syndication;

namespace daily_dev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SourceController : ControllerBase
    {
        private readonly NewsDbContext _context;

        public SourceController(NewsDbContext context)
        {
            _context = context;
        }

        // GET: api/source
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dim_Source>>> GetAllSources()
        {
            return await _context.Dim_Source.ToListAsync();
        }

        // GET: api/source/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Dim_Source>> GetSourceById(int id)
        {
            var source = await _context.Dim_Source.FindAsync(id);

            if (source == null)
            {
                return NotFound();
            }

            return source;
        }

        // POST: api/source
        [HttpPost]
        public async Task<ActionResult<Dim_Source>> CreateSource([FromBody] Dim_Source source)
        {
            if (source == null)
            {
                return BadRequest("Source data is null");
            }

            _context.Dim_Source.Add(source);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSourceById), new { id = source.SourceID }, source);
        }

        // PUT: api/source/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSource(int id, [FromBody] Dim_Source source)
        {
            if (id != source.SourceID)
            {
                return BadRequest("Source ID mismatch");
            }

            var existingSource = await _context.Dim_Source.FindAsync(id);
            if (existingSource == null)
            {
                return NotFound();
            }

            existingSource.SourceName = source.SourceName;
            existingSource.RSS_URL = source.RSS_URL;
            existingSource.LastUpdated = source.LastUpdated;
            existingSource.ArticleCount = source.ArticleCount;
            existingSource.IsActive = source.IsActive;

            _context.Dim_Source.Update(existingSource);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/source/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSource(int id)
        {
            var source = await _context.Dim_Source.FindAsync(id);
            if (source == null)
            {
                return NotFound();
            }

            _context.Dim_Source.Remove(source);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
