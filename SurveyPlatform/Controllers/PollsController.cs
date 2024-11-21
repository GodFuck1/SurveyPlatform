using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SurveyPlatform.API.DTOs.Requests;
using SurveyPlatform.DTOs.Requests;
using SurveyPlatform.DTOs.Responses;


namespace SurveyPlatform.Controllers
{
    [Route("api/polls")]
    [Authorize]
    [ApiController]
    public class PollsController : ControllerBase
    {
        /// <summary>
        /// Получение списка опросов (без результатов)
        /// </summary>
        /// <returns>Список опросов</returns>
        [HttpGet]
        public ActionResult<List<PollDataResponse>> GetPolls()
        {
            return Ok();
        }

        /// <summary>
        /// Получение опроса по id (без результатов)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Опрос</returns>
        [HttpGet("{id}")]
        public ActionResult<PollDataResponse> GetPollById(int id)
        {
            var new_pool = new PollDataResponse
            {
                Id = id,
                Title = "test",
                Description = "test-description",
                CreatedIn = DateTime.Now,
                AuthorID = 1999,
                Options = new List<OptionResponse>
                {
                    new OptionResponse { Id = 1, Content = "otvet 1" },
                    new OptionResponse { Id = 2, Content = "otvet 2" }
                }
            };
            return new_pool;
        }

        /// <summary>
        /// Получение результатов опроса
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Результаты опроса</returns>
        [HttpGet("{id}/results")]
        public IActionResult GetPollResults(int id)
        {
            var pollResultsResponse = new PollResultsResponse
            {
                PollId = id,
                Title = "test",
                Options = new List<OptionResult>
                {
                    new OptionResult { OptionId = 1, Content = "otvet 1", ResponseCount = 10 },
                    new OptionResult { OptionId = 2, Content = "otvet 2", ResponseCount = 20 }
                }
            };
            return Ok(pollResultsResponse);
        }

        /// <summary>
        /// Отправка ответа на опрос
        /// </summary>
        /// <param name="id">ID опроса</param>
        /// <param name="submitResponseRequest">Тело запроса ID ответа и ID пользователя</param>
        /// <returns>Результаты опроса после ответа</returns>
        [HttpPost("{id}/submit-response")]
        public ActionResult<PollResultsResponse> SubmitResponse(int id,[FromForm] SubmitResponseRequest submitResponseRequest)
        {
            //return Ok(submitResponseRequest);
            var pollResultsResponse = new PollResultsResponse
            {
                PollId = id,
                Title = "test",
                Options = new List<OptionResult>
                {
                    new OptionResult { OptionId = 1, Content = "otvet 1", ResponseCount = 10 },
                    new OptionResult { OptionId = 2, Content = "otvet 2", ResponseCount = 20 }
                }
            };
            return Ok(pollResultsResponse);
        }

        /// <summary>
        /// Создание опроса
        /// </summary>
        /// <param name="pollRequest"></param>
        /// <returns>Новый опрос</returns>
        [HttpPost]
        public ActionResult<PollDataResponse> CreatePoll([FromForm] CreatePollRequest pollRequest)
        {
            if (pollRequest.Options.Count < 2)
            {
                return BadRequest();
            }

            var new_pool = new PollDataResponse
            {
                Id = new Random().Next(10,100),
                Title = "test",
                Description = "test-description",
                CreatedIn = DateTime.Now,
                AuthorID = 1999,
                Options = new List<OptionResponse>()
            };
              foreach (var option in pollRequest.Options)
                new_pool.Options.Add(new OptionResponse { Id = new Random().Next(10, 100), Content = option });
            return Ok(new_pool);
        }

        
        /// <summary>
        /// Обновление опроса
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatePollRequest">Тело запроса с новыми Title & Description</param>
        /// <returns>Обновлённый опрос</returns>
        [HttpPut("{id}")]
        public ActionResult<PollDataResponse> UpdatePoll(int id,[FromForm] UpdatePollRequest updatePollRequest)
        {
            var new_pool = new PollDataResponse
            {
                Id = id,
                Title = updatePollRequest.Title,
                Description = updatePollRequest.Description,
                CreatedIn = DateTime.Now,
                AuthorID = 1999,
                Options = new List<OptionResponse>
                {
                    new OptionResponse { Id = 1, Content = "otvet 1" },
                    new OptionResponse { Id = 2, Content = "otvet 2" }
                }
            };
            return new_pool;
        }

        /// <summary>
        /// Удаление опроса
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Статус 200 - удача / Статус 418 - неудачно</returns>
        [HttpDelete("{id}")]
        public ActionResult DeletePoll(int id)
        {
            return StatusCode(418);
            return StatusCode(200);
        }
    }
}
