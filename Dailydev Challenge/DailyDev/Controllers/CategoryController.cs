using DailyDev.Models;
using DailyDev.Repository;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

/*namespace DailyDev.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepo _categoryRepository;

        public CategoryController(CategoryRepo categoryRepository)
        {
            _categoryRepository = categoryRepository;
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
    }
}*/

namespace DailyDev.Controllers
{
    [ApiController]
    [Route("odata/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepo _categoryRepo;

        public CategoryController(CategoryRepo categoryRepository)
        {
            _categoryRepo = categoryRepository;
        }

        [EnableQuery]
        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _categoryRepo.GetAll();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public ActionResult<Category> GetById(int id)
        {
            var category = _categoryRepo.GetById(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public ActionResult Add([FromBody] Category category)
        {
            _categoryRepo.Add(category);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);

        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }
            _categoryRepo.Update(category);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var category = _categoryRepo.GetById(id);
            if (category is null)
            {
                return BadRequest();
            }

            _categoryRepo.Delete(id);

            return NoContent();
        }
    }
}