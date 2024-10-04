using DailyDev.Models;
using DailyDev.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DailyDev.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly ItemRepository _itemRepository;

        public ItemController(ItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Item>> GetAll()
        {
            return Ok(_itemRepository.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Item> GetById(int id)
        {
            var item = _itemRepository.GetById(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public ActionResult Add([FromBody] Item item)
        {
            _itemRepository.Add(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Item item)
        {
            item.Id = id;
            _itemRepository.Update(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _itemRepository.Delete(id);
            return NoContent();
        }
    }

}
