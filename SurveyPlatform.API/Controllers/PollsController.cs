using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyPlatform.API.DTOs.Requests;
using SurveyPlatform.BLL;
using SurveyPlatform.BLL.Exceptions;
using SurveyPlatform.BLL.Models;
using SurveyPlatform.DTOs.Responses;


namespace SurveyPlatform.Controllers
{
    [Route("api/polls")]
    [Authorize]
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
        public async Task<ActionResult<IEnumerable<PollDataResponse>>> GetPolls()
        {
            var pollsData = await _pollService.GetAllPollsAsync();
            var pollsMapped = _mapper.Map<IEnumerable<PollDataResponse>>(pollsData);
            return Ok(pollsMapped);
        }

        /// <summary>
        /// Получение опроса по id (без результатов)
        /// </summary>
        /// <param name="pollId"></param>
        /// <returns>Опрос</returns>
        [HttpGet("{pollId}")]
        public async Task<ActionResult<PollDataResponse>> GetPollById(Guid pollId)
        {
            var pollData = await _pollService.GetPollByIdAsync(pollId);
            var poll = _mapper.Map<PollDataResponse>(pollData);
            return Ok(poll);
        }

        /// <summary>
        /// Получение результатов опроса
        /// </summary>
        /// <param name="pollId"></param>
        /// <returns>Результаты опроса</returns>
        [HttpGet("{pollId}/results")]
        public async Task<ActionResult<PollDataResponse>> GetPollResults(Guid pollId)
        {
            var pollResults = await _pollService.GetResponsesByPollIdAsync(pollId);
            var pollMappedResults = _mapper.Map<PollDataResponse>(pollResults);
            return Ok(pollMappedResults);
        }

        /// <summary>
        /// Отправка ответа на опрос
        /// </summary>
        /// <param name="pollId">ID опроса</param>
        /// <param name="optionId">Тело запроса ID ответа и ID пользователя</param>
        /// <returns>Результаты опроса после ответа</returns>
        [HttpGet("{pollId}/submit-response/{optionId}")]
        public async Task<ActionResult<PollDataResponse>> SubmitResponse(Guid pollId, Guid optionId)
        {
            var poll = await _pollService.GetPollByIdAsync(pollId);
            if (poll == null)
                throw new EntityNotFoundException("Poll not found");

            var pollResults = await _pollService.AddPollResponseAsync(pollId,optionId);
            if (pollResults == null)
                throw new UnauthorizedAccessException("User not found");

            var pollMappedResults = _mapper.Map<PollDataResponse>(pollResults);
            return Ok(pollMappedResults);
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
            await _pollService.CreatePollAsync(newPoll);
            return Ok();
        }


        /// <summary>
        /// Обновление опроса
        /// </summary>
        /// <param name="pollId">Id опроса</param>
        /// <param name="updatePollRequest">Тело запроса с новыми Title & Description</param>
        /// <returns>Обновлённый опрос</returns>
        [HttpPut("{pollId}")]
        public async Task<ActionResult<PollDataResponse>> UpdatePoll([FromRoute] Guid pollId,[FromForm] UpdatePollRequest updatePollRequest)
        {
            var mappedUpdatePollRequest = _mapper.Map<UpdatePollModel>(updatePollRequest);
            var updatedPoll = await _pollService.UpdatePollAsync(pollId,mappedUpdatePollRequest);
            return Ok(updatedPoll);
        }

        /// <summary>
        /// Удаление опроса
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Статус 200 - удача</returns>
        [HttpDelete("{pollId}")]
        public async Task<ActionResult> DeletePoll(Guid pollId)
        {
            await _pollService.DeletePollAsync(pollId);
            return Ok();
        }
    }
}
