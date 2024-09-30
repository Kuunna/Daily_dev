using daily_dev.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace daily_dev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly NewsDbContext _context;

        public UserController(NewsDbContext context)
        {
            _context = context;
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dim_User>>> GetAllUsers()
        {
            return await _context.Dim_User.ToListAsync();
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Dim_User>> GetUserById(int id)
        {
            var user = await _context.Dim_User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/user
        [HttpPost]
        public async Task<ActionResult<Dim_User>> CreateUser([FromBody] Dim_User user)
        {
            if (user == null)
            {
                return BadRequest("User data is null");
            }

            // You might want to hash the password here before saving it
            // Example: user.Password = HashPassword(user.Password);

            _context.Dim_User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = user.UserID }, user);
        }

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Dim_User user)
        {
            if (id != user.UserID)
            {
                return BadRequest("User ID mismatch");
            }

            var existingUser = await _context.Dim_User.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            // Update the user information
            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;  // Remember to hash it
            existingUser.Preferences = user.Preferences;
            existingUser.LastLogin = user.LastLogin; // Assuming this gets updated on login

            _context.Dim_User.Update(existingUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Dim_User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Dim_User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
