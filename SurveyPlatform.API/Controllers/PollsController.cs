using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SurveyPlatform.API.DTOs.Requests;
using SurveyPlatform.BLL;
using SurveyPlatform.BLL.Models;
using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DTOs.Requests;
using SurveyPlatform.DTOs.Requests.Validators;
using SurveyPlatform.DTOs.Responses;


namespace SurveyPlatform.Controllers
{
    [Route("api/polls")]
    //[Authorize]
    [ApiController]
    public class PollsController : ControllerBase
    {
        private readonly PollService _pollService;
        private readonly IMapper _mapper;

        public PollsController(PollService pollService,IMapper mapper)
        {
            _pollService = pollService;
            _mapper = mapper;
        }
        /// <summary>
        /// Получение списка опросов (без результатов)
        /// </summary>
        /// <returns>Список опросов</returns>
        [HttpGet]
        public async Task<ActionResult<List<PollDataResponse>>> GetPolls()
        {
            var pollsData = await _pollService.GetAllPolls();
            return Ok(pollsData);
        }

        /// <summary>
        /// Получение опроса по id (без результатов)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Опрос</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PollDataResponse>> GetPollById(Guid id)
        {
            var pollData = await _pollService.GetPollById(id);
            return Ok(pollData);
        }

        /// <summary>
        /// Получение результатов опроса
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Результаты опроса</returns>
        [HttpGet("{id}/results")]
        public IActionResult GetPollResults(Guid id)
        {
            var pollResults = _pollService.GetResponsesByPollId(id);
            return Ok(pollResults);
        }

        /// <summary>
        /// Отправка ответа на опрос
        /// </summary>
        /// <param name="id">ID опроса</param>
        /// <param name="submitResponseRequest">Тело запроса ID ответа и ID пользователя</param>
        /// <returns>Результаты опроса после ответа</returns>
        [HttpPost("{id}/submit-response")]
        public async Task<ActionResult<PollResultsResponse>> SubmitResponse(Guid id,[FromForm] SubmitResponseRequest submitResponseRequest)
        {
            await _pollService.AddPollResponse(null);
            return Ok();
        }

        /// <summary>
        /// Создание опроса
        /// </summary>
        /// <param name="pollRequest"></param>
        /// <returns>Новый опрос</returns>
        [HttpPost]
        public async Task<ActionResult<PollDataResponse>> CreatePoll([FromForm] CreatePollRequest pollRequest)
        {
            var newPoll = _mapper.Map<PollModel>(pollRequest);
            await _pollService.CreatePoll(newPoll);
            return Ok();
        }

        
        /// <summary>
        /// Обновление опроса
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatePollRequest">Тело запроса с новыми Title & Description</param>
        /// <returns>Обновлённый опрос</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<PollDataResponse>> UpdatePoll(Guid id,[FromForm] UpdatePollRequest updatePollRequest)
        {
            await _pollService.UpdatePoll(null);
            return Ok();
        }

        /// <summary>
        /// Удаление опроса
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Статус 200 - удача</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePoll(Guid id)
        {
            await _pollService.DeletePoll(id);
            return Ok();
        }
    }
}
