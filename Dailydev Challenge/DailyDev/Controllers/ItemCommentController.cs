using DailyDev.Models;
using DailyDev.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DailyDev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemCommentController : ControllerBase
    {
        private readonly ItemCommentRepository _itemCommentRepository;

        public ItemCommentController(ItemCommentRepository itemCommentRepository)
        {
            _itemCommentRepository = itemCommentRepository;
        }

        [HttpGet("{itemId}")]
        public async Task<ActionResult<IEnumerable<ItemComment>>> GetCommentsByItem(int itemId)
        {
            var comments = await _itemCommentRepository.GetByItemId(itemId);
            return Ok(comments);
        }

        [HttpGet("comment/{id}")]
        public async Task<ActionResult<ItemComment>> GetCommentById(int id)
        {
            var comment = await _itemCommentRepository.GetById(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] ItemComment comment)
        {
            comment.CreateAt = DateTime.UtcNow;
            await _itemCommentRepository.Add(comment);
            return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, comment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] ItemComment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            var existingComment = await _itemCommentRepository.GetById(id);
            if (existingComment == null)
            {
                return NotFound();
            }

            await _itemCommentRepository.Update(comment);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _itemCommentRepository.GetById(id);
            if (comment == null)
            {
                return NotFound();
            }

            await _itemCommentRepository.Delete(id);
            return NoContent();
        }
    }
}
