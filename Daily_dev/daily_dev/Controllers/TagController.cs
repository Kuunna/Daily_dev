using daily_dev.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace daily_dev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly NewsDbContext _context;

        public TagController(NewsDbContext context)
        {
            _context = context;
        }

        // GET: api/tag
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dim_Tag>>> GetAllTags()
        {
            return await _context.Dim_Tag.ToListAsync();
        }

        // GET: api/tag/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Dim_Tag>> GetTagById(int id)
        {
            var tag = await _context.Dim_Tag.FindAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            return tag;
        }

        // POST: api/tag
        [HttpPost]
        public async Task<ActionResult<Dim_Tag>> CreateTag([FromBody] Dim_Tag tag)
        {
            if (tag == null)
            {
                return BadRequest("Tag data is null");
            }

            _context.Dim_Tag.Add(tag);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTagById), new { id = tag.TagID }, tag);
        }

        // PUT: api/tag/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTag(int id, [FromBody] Dim_Tag tag)
        {
            if (id != tag.TagID)
            {
                return BadRequest("Tag ID mismatch");
            }

            var existingTag = await _context.Dim_Tag.FindAsync(id);
            if (existingTag == null)
            {
                return NotFound();
            }

            // Update the tag information
            existingTag.TagName = tag.TagName;
            existingTag.TagDescription = tag.TagDescription;

            _context.Dim_Tag.Update(existingTag);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/tag/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _context.Dim_Tag.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            _context.Dim_Tag.Remove(tag);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
