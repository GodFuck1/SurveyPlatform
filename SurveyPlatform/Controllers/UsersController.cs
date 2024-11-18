using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SurveyPlatform.Models.Requests;
using SurveyPlatform.Models.Responses;

namespace SurveyPlatform.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : Controller
    {
        [HttpPost]
        public ActionResult<Guid> Register([FromBody] RegisterUserRequest request)
        {
            var addedUserId = Guid.NewGuid();
            return Ok(addedUserId);
        }
        
        [HttpPost("login")]
        public IActionResult LogIn([FromBody] LoginUserRequest request)
        {
            return Ok();
        }

        [HttpGet]
        public ActionResult<List<UserResponse>> GetUsers()
        {
            return Ok();
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
