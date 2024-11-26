using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SurveyPlatform.API.DTOs.Requests;
using SurveyPlatform.BLL;
using SurveyPlatform.BLL.Exceptions;
using SurveyPlatform.BLL.Models;
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
        public async Task<ActionResult<IEnumerable<PollDataResponse>>> GetPolls()
        {
            var pollsData = await _pollService.GetAllPollsAsync();
            var pollsMapped = _mapper.Map<IEnumerable<PollDataResponse>>(pollsData);
            return Ok(pollsMapped);
        }

        /// <summary>
        /// Получение опроса по id (без результатов)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Опрос</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PollDataResponse>> GetPollById(Guid id)
        {
            var pollData = await _pollService.GetPollByIdAsync(id);
            var poll = _mapper.Map<PollDataResponse>(pollData);
            return Ok(poll);
        }

        /// <summary>
        /// Получение результатов опроса
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Результаты опроса</returns>
        [HttpGet("{id}/results")]
        public async Task<ActionResult<PollDataResponse>> GetPollResults(Guid id)
        {
            var pollResults = await _pollService.GetResponsesByPollIdAsync(id);
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
            var pollResults = await _pollService.AddPollResponseAsync(pollId,optionId);
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
        /// <param name="id"></param>
        /// <param name="updatePollRequest">Тело запроса с новыми Title & Description</param>
        /// <returns>Обновлённый опрос</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<PollDataResponse>> UpdatePoll(Guid id,[FromForm] UpdatePollRequest updatePollRequest)
        {
            var mappedUpdatePollRequest = _mapper.Map<UpdatePollModel>(updatePollRequest);
            var result = await _pollService.UpdatePollAsync(mappedUpdatePollRequest);
            if (result == false)
            {
                throw new EntityNotFoundException($"Poll {updatePollRequest.PollId} not found");
            }
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
            await _pollService.DeletePollAsync(id);
            return Ok();
        }
    }
}
