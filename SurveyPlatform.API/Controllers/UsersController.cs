using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyPlatform.API.DTOs.Requests;
using SurveyPlatform.BLL.Models;
using SurveyPlatform.Business;
using SurveyPlatform.DTOs.Requests.Validators;
using SurveyPlatform.DTOs.Responses;

namespace SurveyPlatform.Controllers
{
    [Route("api/users")]
    [Authorize]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public UsersController(UserService userService,IMapper Mapper)
        {
            _userService = userService;
            _mapper = Mapper;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterUserRequest request)
        {
            var validator = new RegisterUserRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var newUserData = _mapper.Map<RegisterUserRequest, UserRegisterModel>(request);
            var userResponse = await _userService.RegisterUserAsync(newUserData);
            if (userResponse != null) return Ok(userResponse);
            else return Conflict("Email already has in DB");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginUserRequest request)
        {
            var validator = new LoginUserRequestValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var userModel = _mapper.Map<LoginUserRequest, UserLoginModel>(request);
            var loginResponse = await _userService.LoginUserAsync(userModel);
            if (loginResponse == string.Empty) return Unauthorized("Email Or Password Is Incorrect");
            return Ok(loginResponse);
        }

        [HttpGet]
        public ActionResult<List<UserResponse>> GetUsers()
        {
            var users = _userService.GetAllUsers();
            var allUsers = _mapper.Map<List<UserResponse>>(users);
            return Ok(allUsers);
        }

        [HttpGet("{id}")]
        public ActionResult<UserResponse> GetUserByID([FromRoute] Guid id)
        {
            var users = _userService.GetUserByIdAsync(id);
            var userMapped = _mapper.Map<UserResponse>(users);
            return Ok(userMapped);
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
