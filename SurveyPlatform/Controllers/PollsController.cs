using Microsoft.AspNetCore.Mvc;
using SurveyPlatform.Models.Requests;
using SurveyPlatform.Models.Responses;


namespace SurveyPlatform.Controllers
{
    [Route("api/polls")]
    [ApiController]
    public class PollsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<PollResponse>> GetPolls()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public ActionResult<PollResponse> GetPoolById(int id)
        {
            var new_pool = new PollResponse();
            new_pool.Id = id;
            new_pool.CreatedIn = DateTime.Now;
            return new_pool;
        }

        [HttpPost]
        public ActionResult<PollResponse> CreatePoll([FromForm] PollRequest pollRequest)
        {
            if (pollRequest.Options.Count < 2)
            {
                return BadRequest();
            }
            return Ok(pollRequest);
        }

        [HttpPost("response")]
        public ActionResult<ResponseRequest> ChooseResponse([FromForm] ResponseRequest responseRequest)
        {
            return Ok(responseRequest);
        }

        [HttpPut("{id}")]
        public IActionResult ChooseResponse(int id,[FromForm] UpdatePollRequest updatePollRequest)
        {
            return Ok(updatePollRequest);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePoll(int id)
        {
            return StatusCode(418);
        }
    }
}
