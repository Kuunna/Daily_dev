using DailyDev.Models;
using DailyDev.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace DailyDev.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly ProviderRepository _providerRepository;

        public CategoryController(CategoryRepository categoryRepository, ProviderRepository providerRepository)
        {
            _categoryRepository = categoryRepository;
            _providerRepository = providerRepository;
        }

        [HttpGet("{id}")]
        public ActionResult<Category> GetById(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public ActionResult Add([FromBody] Category category)
        {
            _categoryRepository.Add(category);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Category category)
        {
            category.Id = id;
            _categoryRepository.Update(category);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _categoryRepository.Delete(id);
            return NoContent();
        }

        [EnableQuery]
        [HttpGet]
        public IQueryable<Category> Get()
        {
            // Convert IEnumerable to IQueryable for OData to work
            var categories = _categoryRepository.GetAll().AsQueryable();
            return categories;
        }
    }
}
