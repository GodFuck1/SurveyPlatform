using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyPlatform.API.DTOs.Requests;
using SurveyPlatform.BLL.Exceptions;
using SurveyPlatform.BLL.Interfaces;
using SurveyPlatform.BLL.Models;
using SurveyPlatform.DTOs.Responses;
using System.ComponentModel;


namespace SurveyPlatform.Controllers
{
    [Route("api/polls")]
    [Authorize]
    [ApiController]
    public class PollsController(
            IPollService pollService, 
            IMapper mapper
        ) : ControllerBase
    {
        /// <summary>
        /// Получение списка опросов (без результатов)
        /// </summary>
        /// <returns>Список опросов</returns>
        [HttpGet]
        [EndpointDescription("Получение списка опросов (без результатов)")]
        public async Task<ActionResult<IEnumerable<PollDataResponse>>> GetPolls()
        {
            var pollsData = await pollService.GetAllPollsAsync();
            var pollsMapped = mapper.Map<IEnumerable<PollDataResponse>>(pollsData);
            return Ok(pollsMapped);
        }

        /// <summary>
        /// Получение опроса по id (без результатов)
        /// </summary>
        /// <param name="pollId"></param>
        /// <returns>Опрос</returns>
        [HttpGet("{pollId}")]
        [EndpointDescription("Получение опроса по id (без результатов)")]
        public async Task<ActionResult<PollDataResponse>> GetPollById(Guid pollId)
        {
            var pollData = await pollService.GetPollByIdAsync(pollId);
            var poll = mapper.Map<PollDataResponse>(pollData);
            return Ok(poll);
        }

        /// <summary>
        /// Получение результатов опроса
        /// </summary>
        /// <param name="pollId"></param>
        /// <returns>Результаты опроса</returns>
        [HttpGet("{pollId}/results")]
        [EndpointDescription("Получение результатов опроса")]
        public async Task<ActionResult<PollDataResponse>> GetPollResults(Guid pollId)
        {
            var pollResults = await pollService.GetResponsesByPollIdAsync(pollId);
            var pollMappedResults = mapper.Map<PollDataResponse>(pollResults);
            return Ok(pollMappedResults);
        }

        /// <summary>
        /// Отправка ответа на опрос, UserID берётся из контекста(токен)
        /// </summary>
        /// <param name="pollId">ID опроса</param>
        /// <param name="optionId">ID ответа</param>
        /// <returns>Результаты опроса после ответа</returns>
        [HttpGet("{pollId}/submit-response/{optionId}")]
        [EndpointDescription("Отправка ответа на опрос, UserID берётся из контекста(токен)")]
        public async Task<ActionResult<PollDataResponse>> SubmitResponse(Guid pollId, Guid optionId)
        {
            var pollResult = await pollService.AddPollResponseAsync(pollId,optionId);
            var result = mapper.Map<PollDataResponse>(pollResult);
            return Ok(result);
        }

        /// <summary>
        /// Создание опроса
        /// </summary>
        /// <param name="pollRequest"></param>
        /// <returns>Новый опрос</returns>
        [HttpPost]
        [EndpointDescription("Создание опроса")]
        public async Task<ActionResult<PollDataResponse>> CreatePoll([FromForm] CreatePollRequest pollRequest)
        {
            var newPoll = mapper.Map<PollModel>(pollRequest);
            var createdPoll = await pollService.CreatePollAsync(newPoll);
            var result = mapper.Map<PollDataResponse>(createdPoll);
            return Ok(result);
        }


        /// <summary>
        /// Обновление опроса
        /// </summary>
        /// <param name="pollId">Id опроса</param>
        /// <param name="updatePollRequest">Тело запроса с новыми Title & Description</param>
        /// <returns>Обновлённый опрос</returns>
        [HttpPut("{pollId}")]
        [EndpointDescription("Обновление опроса")]
        public async Task<ActionResult<PollDataResponse>> UpdatePoll([FromRoute] Guid pollId,[FromForm] UpdatePollRequest updatePollRequest)
        {
            var mappedUpdatePollRequest = mapper.Map<UpdatePollModel>(updatePollRequest);
            var updatedPoll = await pollService.UpdatePollAsync(pollId,mappedUpdatePollRequest);
            var result = mapper.Map<PollDataResponse>(updatedPoll);
            return Ok(result);
        }

        /// <summary>
        /// Удаление опроса
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Статус 200 - удача</returns>
        [HttpDelete("{pollId}")]
        [EndpointDescription("Удаление опроса")]
        public async Task<ActionResult> DeletePoll(Guid pollId)
        {
            await pollService.DeletePollAsync(pollId);
            return Ok();
        }
    }
}
