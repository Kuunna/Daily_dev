using daily_dev.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace daily_dev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InteractionController : ControllerBase
    {
        private readonly NewsDbContext _context;

        public InteractionController(NewsDbContext context)
        {
            _context = context;
        }

        // GET: api/interaction
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fact_Article_Interaction>>> GetAllInteractions()
        {
            return await _context.Fact_Article_Interaction.ToListAsync();
        }

        // GET: api/interaction/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Fact_Article_Interaction>> GetInteractionById(int id)
        {
            var interaction = await _context.Fact_Article_Interaction.FindAsync(id);

            if (interaction == null)
            {
                return NotFound();
            }

            return interaction;
        }

        // POST: api/interaction
        [HttpPost]
        public async Task<ActionResult<Fact_Article_Interaction>> CreateInteraction([FromBody] Fact_Article_Interaction interaction)
        {
            if (interaction == null)
            {
                return BadRequest("Interaction data is null");
            }

            _context.Fact_Article_Interaction.Add(interaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInteractionById), new { id = interaction.InteractionID }, interaction);
        }

        // PUT: api/interaction/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInteraction(int id, [FromBody] Fact_Article_Interaction interaction)
        {
            if (id != interaction.InteractionID)
            {
                return BadRequest("Interaction ID mismatch");
            }

            var existingInteraction = await _context.Fact_Article_Interaction.FindAsync(id);
            if (existingInteraction == null)
            {
                return NotFound();
            }

            // Update interaction details
            existingInteraction.InteractionType = interaction.InteractionType;
            existingInteraction.CommentText = interaction.CommentText;
            existingInteraction.UpvoteCount = interaction.UpvoteCount;

            _context.Fact_Article_Interaction.Update(existingInteraction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/interaction/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInteraction(int id)
        {
            var interaction = await _context.Fact_Article_Interaction.FindAsync(id);
            if (interaction == null)
            {
                return NotFound();
            }

            _context.Fact_Article_Interaction.Remove(interaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
