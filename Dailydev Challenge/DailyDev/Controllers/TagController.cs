using DailyDev.Models;
using DailyDev.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DailyDev.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagController : ControllerBase
    {
        private readonly TagRepository _tagRepository;

        public TagController(TagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        [HttpGet("{id}")]
        public ActionResult<Tag> GetById(int id)
        {
            var tag = _tagRepository.GetById(id);
            if (tag == null)
                return NotFound();

            return Ok(tag);
        }

        [HttpPost]
        public ActionResult Add([FromBody] Tag tag)
        {
            _tagRepository.Add(tag);
            return CreatedAtAction(nameof(GetById), new { id = tag.Id }, tag);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Tag tag)
        {
            tag.Id = id;
            _tagRepository.Update(tag);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _tagRepository.Delete(id);
            return NoContent();
        }
    }

}
