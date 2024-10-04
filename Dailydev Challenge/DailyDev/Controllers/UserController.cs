using DailyDev.Models;
using DailyDev.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DailyDev.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            return Ok(_userRepository.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public ActionResult Add([FromBody] User user)
        {
            _userRepository.Add(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] User user)
        {
            user.Id = id;
            _userRepository.Update(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _userRepository.Delete(id);
            return NoContent();
        }
    }

}
