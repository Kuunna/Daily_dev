using DailyDev.Models;
using DailyDev.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DailyDev.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly ItemRepo _itemRepository;
        private readonly HttpClient _httpClient;
        private readonly CategoryRepo _categoryRepository;

        public ItemController(ItemRepo itemRepository, HttpClient httpClient, CategoryRepo categoryRepository)
        {
            _itemRepository = itemRepository;
            _httpClient = httpClient;
            _categoryRepository = categoryRepository;
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

        [HttpPatch("{id}")]
        public ActionResult Update(int id, [FromBody] Item item)
        {
            item.Id = id;
            _itemRepository.Update(item);
            return NoContent();
        }

        [HttpPut]

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _itemRepository.Delete(id);
            return NoContent();
        }
    }
}
