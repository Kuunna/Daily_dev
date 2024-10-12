using DailyDev.Models;
using DailyDev.Repositories;
using DailyDev.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace DailyDev.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly UserCategoryRepository _userCategoryRepository;
        private readonly UserTagRepository _userTagRepository;
        private readonly UserProviderRepository _userProviderRepository;

        public UserController(UserRepository userRepository, UserCategoryRepository userCategoryRepository, 
                              UserTagRepository userTagRepository, UserProviderRepository userProviderRepository)
        {
            _userRepository = userRepository;
            _userCategoryRepository = userCategoryRepository;
            _userTagRepository = userTagRepository;
            _userProviderRepository = userProviderRepository;
        }

        // Đăng ký tài khoản
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDto userRegisterDto)
        {
            var user = new User
            {
                Name = userRegisterDto.Name,
                Password = HashPassword(userRegisterDto.Password),  // Hash mật khẩu
                Email = userRegisterDto.Email,
                FullName = userRegisterDto.FullName,
                DOB = userRegisterDto.DOB
            };

            if (_userRepository.Register(user))
            {
                return Ok(new { message = "User registered successfully" });
            }
            return BadRequest(new { message = "Failed to register user" });
        }

        // Đăng nhập tài khoản
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto userLoginDto)
        {
            var user = _userRepository.Login(userLoginDto.Name, HashPassword(userLoginDto.Password));
            if (user != null)
            {
                return Ok(user);
            }
            return Unauthorized(new { message = "Invalid credentials" });
        }

        // Cập nhật thông tin người dùng
        [HttpPut("update/{userId}")]
        public IActionResult UpdateUser(int userId, [FromBody] UserUpdateDto userUpdateDto)
        {
            var user = new User
            {
                FullName = userUpdateDto.FullName,
                Email = userUpdateDto.Email,
                DOB = userUpdateDto.DOB
            };

            if (_userRepository.UpdateUser(userId, user))
            {
                return Ok(new { message = "User updated successfully" });
            }
            return BadRequest(new { message = "Failed to update user" });
        }

        // Thêm sở thích Category
        [HttpPost("add-category-preference")]
        public IActionResult AddCategoryPreference([FromBody] UserCategoryDto userCategoryDto)
        {
            var userCategory = new UserCategory
            {
                UserId = userCategoryDto.UserId,
                CategoryId = userCategoryDto.CategoryId
            };
            _userCategoryRepository.Add(userCategory);
            return Ok(new { message = "Category preference added successfully" });
        }

        // Thêm sở thích Tag
        [HttpPost("add-tag-preference")]
        public IActionResult AddTagPreference([FromBody] UserTagDto userTagDto)
        {
            var userTag = new UserTag
            {
                UserId = userTagDto.UserId,
                TagId = userTagDto.TagId
            };
            _userTagRepository.Add(userTag);
            return Ok(new { message = "Tag preference added successfully" });
        }

        // Thêm sở thích Provider
        [HttpPost("add-provider-preference")]
        public IActionResult AddProviderPreference([FromBody] UserProviderDto userProviderDto)
        {
            var userProvider = new UserProvider
            {
                UserId = userProviderDto.UserId,
                ProviderId = userProviderDto.ProviderId
            };
            _userProviderRepository.Add(userProvider);
            return Ok(new { message = "Provider preference added successfully" });
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
        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            _userRepository.Delete(id);
            return NoContent();
        }

        private string HashPassword(string password)
        {
            // Hàm băm mật khẩu
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        public class UserRegisterDto
        {
            public string Name { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public string FullName { get; set; }
            public DateTime DOB { get; set; }
        }
        public class UserLoginDto
        {
            public string Name { get; set; }
            public string Password { get; set; }
        }
        public class UserProviderDto
        {
            public int UserId { get; set; }
            public int ProviderId { get; set; }
        }
        public class UserCategoryDto
        {
            public int UserId { get; set; }
            public int CategoryId { get; set; }
        }
        public class UserTagDto
        {
            public int UserId { get; set; }
            public int TagId { get; set; }
        }
    }
}
