using DailyDev.Models;
using DailyDev.Repositories;
using DailyDev.Repository;
using DailyDev.Dto;
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
        private readonly UserLikeRepository _userLikeRepository;


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
                Password = _userRepository.HashPassword(userRegisterDto.Password),  // Hash mật khẩu
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
            var user = _userRepository.Login(userLoginDto.Name, _userRepository.HashPassword(userLoginDto.Password));
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
        [HttpPost("register-category")]
        public IActionResult RegisterCategory([FromBody] UserCategoryDto userCategoryDto)
        {
            var userCategory = new UserCategory
            {
                UserId = userCategoryDto.UserId,
                CategoryId = userCategoryDto.CategoryId
            };
            _userCategoryRepository.Add(userCategory);
            return Ok(new { message = "Category registered successfully" });
        }

        // Xóa sở thích Category
        [HttpDelete("unregister-category")]
        public IActionResult UnregisterCategory([FromBody] UserCategoryDto userCategoryDto)
        {
            _userCategoryRepository.Delete(userCategoryDto.UserId, userCategoryDto.CategoryId);
            return Ok(new { message = "Category unregistered successfully" });
        }


        // Thêm sở thích Tag
        [HttpPost("register-tag")]
        public IActionResult RegisterTag([FromBody] UserTagDto userTagDto)
        {
            var userTag = new UserTag
            {
                UserId = userTagDto.UserId,
                TagId = userTagDto.TagId
            };
            _userTagRepository.Add(userTag);
            return Ok(new { message = "Tag registered successfully" });
        }

        // Xóa sở thích Tag
        [HttpDelete("unregister-tag")]
        public IActionResult UnregisterTag([FromBody] UserTagDto userTagDto)
        {
            _userTagRepository.Delete(userTagDto.UserId, userTagDto.TagId);
            return Ok(new { message = "Provider unregistered successfully" });
        }

        // Thêm sở thích Provider
        [HttpPost("provider")]
        public IActionResult RegisterProvider([FromBody] UserProviderDto userProviderDto)
        {
            var userProvider = new UserProvider
            {
                UserId = userProviderDto.UserId,
                ProviderId = userProviderDto.ProviderId
            };
            _userProviderRepository.Add(userProvider);
            return Ok(new { message = "Provider registered successfully" });
        }

        // Xóa sở thích Provider
        [HttpDelete("unregister-provider")]
        public IActionResult UnregisterProvider([FromBody] UserProviderDto userProviderDto)
        {
            _userProviderRepository.Delete(userProviderDto.UserId, userProviderDto.ProviderId);
            return Ok(new { message = "Provider unregistered successfully" });
        }


        // Lấy thông tin user và các sở thích của họ
        [HttpGet("preferences/{userId}")]
        public IActionResult GetUserPreferences(int userId)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var userProviders = _userProviderRepository.GetByUserId(userId);
            var userCategories = _userCategoryRepository.GetByUserId(userId);
            var userTags = _userTagRepository.GetByUserId(userId);

            return Ok(new
            {
                user = user,
                providers = userProviders,
                categories = userCategories,
                tags = userTags
            });
        }

        // Thích tin tức
        [HttpPost("like-news")]
        public IActionResult LikeNews([FromBody] UserLikeDto userLikeDto)
        {
            var userLike = new UserLike
            {
                UserId = userLikeDto.UserId,
                ItemId = userLikeDto.ItemId
            };
            _userLikeRepository.Add(userLike);
            return Ok(new { message = "News liked successfully" });
        }

        // Bỏ thích tin tức
        [HttpDelete("unlike-news")]
        public IActionResult UnlikeNews([FromBody] UserLikeDto userLikeDto)
        {
            _userLikeRepository.Delete(userLikeDto.UserId, userLikeDto.ItemId);
            return Ok(new { message = "News unliked successfully" });
        }


        /*        [HttpGet]
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
                }*/
    }
}
