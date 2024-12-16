using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyPlatform.API.Attributes;
using SurveyPlatform.API.DTOs.Requests;
using SurveyPlatform.BLL.Interfaces;
using SurveyPlatform.BLL.Models;
using SurveyPlatform.Core;
using SurveyPlatform.DTOs.Responses;

namespace SurveyPlatform.Controllers
{
    [Route("api/users")]
    [CustomAuthorize([Roles.User, Roles.Moderator, Roles.Admin, Roles.SuperAdmin])]
    [ApiController]
    public class UsersController(
            IUserService userService, 
            IMapper mapper
        ) : Controller
    {

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterUserRequest request)
        {
            var newUserData = mapper.Map<RegisterUserRequest, UserRegisterModel>(request);
            var userResponse = await userService.RegisterUserAsync(newUserData);
            if (userResponse != null) return Ok(userResponse);
            else return Conflict("Email already has in DB");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginUserRequest request)
        {
            var userModel = mapper.Map<LoginUserRequest, UserLoginModel>(request);
            var loginResponse = await userService.LoginUserAsync(userModel);
            if (loginResponse == string.Empty) return Unauthorized("Email Or Password Is Incorrect");
            return Ok(loginResponse);
        }

        [HttpGet]
        public async Task<ActionResult<List<UserResponse>>> GetUsers()
        {
            var users = await userService.GetAllUsersAsync();
            var allUsers = mapper.Map<List<UserResponse>>(users);
            return Ok(allUsers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUserByID([FromRoute] Guid id)
        {
            var users = await userService.GetUserByIdAsync(id);
            var userMapped = mapper.Map<UserResponse>(users);
            return Ok(userMapped);
        }

        [HttpGet("{id}/responses")]
        public async Task<ActionResult<UserResponsesResponse>> GetUserResponses([FromRoute] Guid id)
        {
            var users = await userService.GetUserResponsesByIdAsync(id);
            var userMapped = mapper.Map<UserResponsesResponse>(users);
            return Ok(userMapped);
        }

        [HttpGet("{id}/polls")]
        public async Task<ActionResult<UserPollsResponse>> GetUserPolls([FromRoute] Guid id)
        {
            var users = await userService.GetUserPollsByIdAsync(id);
            var userMapped = mapper.Map<UserPollsResponse>(users);
            return Ok(userMapped);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponse>> UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserRequest request)
        {
            var userMapped = mapper.Map<UpdateUserModel>(request);
            var updatedUser = await userService.UpdateUserAsync(id,userMapped);
            var updatedUserMapped = mapper.Map<UserResponse>(updatedUser);
            return Ok(updatedUserMapped);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> DeleteUser([FromRoute] Guid id)
        {
            await userService.DeleteUserAsync(id);
            return Ok();
        }

        [HttpPatch("{id}/reactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ReActivateUser([FromRoute] Guid id)
        {
            await userService.ChangeUserActivatedAsync(id);
            return Ok();
        }
    }
}
