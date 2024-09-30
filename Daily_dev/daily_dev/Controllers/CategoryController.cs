using daily_dev.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace daily_dev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly NewsDbContext _context;

        public CategoryController(NewsDbContext context)
        {
            _context = context;
        }

        // GET: api/category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dim_Category>>> GetAllCategories()
        {
            return await _context.Dim_Category.ToListAsync();
        }

        // GET: api/category/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Dim_Category>> GetCategoryById(int id)
        {
            var category = await _context.Dim_Category.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // POST: api/category
        [HttpPost]
        public async Task<ActionResult<Dim_Category>> CreateCategory([FromBody] Dim_Category category)
        {
            if (category == null)
            {
                return BadRequest("Category data is null");
            }

            _context.Dim_Category.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.CategoryID }, category);
        }

        // PUT: api/category/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Dim_Category category)
        {
            if (id != category.CategoryID)
            {
                return BadRequest("Category ID mismatch");
            }

            var existingCategory = await _context.Dim_Category.FindAsync(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            // Update the existing category
            existingCategory.CategoryName = category.CategoryName;
            existingCategory.CategoryDescription = category.CategoryDescription;

            _context.Entry(existingCategory).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/category/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Dim_Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Dim_Category.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
