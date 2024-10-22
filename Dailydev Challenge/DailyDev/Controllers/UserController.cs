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
        private readonly UserRepo _userRepository;
        private readonly UserCategoryRepo _userCategoryRepository;
        private readonly UserTagRepo _userTagRepository;
        private readonly UserProviderRepo _userProviderRepository;
        private readonly UserItemRepo _userItemRepository;


        public UserController(UserRepo userRepository, UserCategoryRepo userCategoryRepository, 
                              UserTagRepo userTagRepository, UserProviderRepo userProviderRepository)
        {
            _userRepository = userRepository;
            _userCategoryRepository = userCategoryRepository;
            _userTagRepository = userTagRepository;
            _userProviderRepository = userProviderRepository;
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDto userRegisterDto)
        {
            var user = new User
            {
                Name = userRegisterDto.Name,
                Password = _userRepository.HashPassword(userRegisterDto.Password),
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

        [HttpDelete("unregister-category")]
        public IActionResult UnregisterCategory([FromBody] UserCategoryDto userCategoryDto)
        {
            _userCategoryRepository.Delete(userCategoryDto.UserId, userCategoryDto.CategoryId);
            return Ok(new { message = "Category unregistered successfully" });
        }

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

        [HttpDelete("unregister-tag")]
        public IActionResult UnregisterTag([FromBody] UserTagDto userTagDto)
        {
            _userTagRepository.Delete(userTagDto.UserId, userTagDto.TagId);
            return Ok(new { message = "Provider unregistered successfully" });
        }

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

        [HttpDelete("unregister-provider")]
        public IActionResult UnregisterProvider([FromBody] UserProviderDto userProviderDto)
        {
            _userProviderRepository.Delete(userProviderDto.UserId, userProviderDto.ProviderId);
            return Ok(new { message = "Provider unregistered successfully" });
        }

        [HttpGet("hobbies/{userId}")]
        public IActionResult GetUserHobbies(int userId)
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
         
        [HttpPost("like-news")]
        public IActionResult LikeNews([FromBody] UserItemDto userItemDto)
        {
            _userItemRepository.LikeItem(userItemDto.UserId, userItemDto.ItemId, true);
            return Ok(new { message = "News liked successfully" });
        }

        [HttpPost("unlike-news")]
        public IActionResult UnlikeNews([FromBody] UserItemDto userItemDto)
        {
            _userItemRepository.LikeItem(userItemDto.UserId, userItemDto.ItemId, false);
            return Ok(new { message = "News unliked successfully" });
        }

        [HttpPost("bookmark-news")]
        public IActionResult BookmarkNews([FromBody] UserItemDto userItemDto)
        {
            _userItemRepository.BookmarkItem(userItemDto.UserId, userItemDto.ItemId, true);
            return Ok(new { message = "News bookmarked successfully" });
        }

        [HttpPost("unbookmark-news")]
        public IActionResult UnbookmarkNews([FromBody] UserItemDto userItemDto)
        {
            _userItemRepository.BookmarkItem(userItemDto.UserId, userItemDto.ItemId, false);
            return Ok(new { message = "News unbookmarked successfully" });
        } 

        [HttpDelete("{id}")]
        public ActionResult DeleteUserById(int id)
        {
            _userRepository.Delete(id);
            return NoContent();
        }
    }
}
