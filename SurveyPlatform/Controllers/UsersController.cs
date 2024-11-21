using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SurveyPlatform.BLL.Models;
using SurveyPlatform.Business;
using SurveyPlatform.DTOs.Requests;
using SurveyPlatform.DTOs.Responses;

namespace SurveyPlatform.Controllers
{
    [Route("api/users")]
    [Authorize]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly UserService _userService;
        private readonly IMapper _Mapper;

        public UsersController(UserService userService,IMapper Mapper)
        {
            _userService = userService;
            _Mapper = Mapper;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterUserRequest request)
        {
            var newUserData = _Mapper.Map<RegisterUserRequest, UserModel>(request);
            var userResponse = await _userService.RegisterUserAsync(newUserData);
            return Ok(userResponse);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginUserRequest request)
        {
            var userModel = _Mapper.Map<LoginUserRequest, UserModel>(request);
            var loginResponse = await _userService.LoginUserAsync(userModel);
            if(loginResponse == string.Empty) return Unauthorized("Email Or Password Is Incorrect");
            return Ok(loginResponse);
        }

        [HttpGet]
        public ActionResult<List<UserResponse>> GetUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserByID([FromRoute] Guid id)
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserRequest request)
        {
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser([FromRoute] Guid id)
        {
            return NoContent();
        }

        [HttpPatch("{id}/deactivate")]
        public IActionResult DeactivateUser([FromRoute] Guid id)
        {
            return NoContent();
        }
    }
}
