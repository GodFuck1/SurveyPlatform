using AutoMapper;
using Microsoft.AspNetCore.Http;
using SurveyPlatform.BLL.Exceptions;
using SurveyPlatform.BLL.Helpers;
using SurveyPlatform.BLL.Interfaces;
using SurveyPlatform.BLL.Models;
using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DAL.Interfaces;
namespace SurveyPlatform.BLL.Services;
public class PollService(
        IPollRepository pollRepository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        IOptionRepository optionRepository,
        IUserService userService
    ) : IPollService
{
    public async Task<PollModel> GetPollByIdAsync(Guid id)
    {
        var poll = await pollRepository.GetPollByIdAsync(id);
        if (poll == null)
            throw new EntityNotFoundException($"Poll {id} not found");
        var pollMapped = mapper.Map<PollModel>(poll);
        return pollMapped;
    }

    public async Task<IEnumerable<PollModel>> GetAllPollsAsync()
    {
        var polls = await pollRepository.GetAllPollsAsync();
        var pollsMapped = mapper.Map<IEnumerable<PollModel>>(polls);
        return pollsMapped;
    }

    public async Task<PollModel> CreatePollAsync(PollModel poll)
    {
        var pollEntity = mapper.Map<Poll>(poll);
        pollEntity.CreatedAt = DateTime.UtcNow;
        await pollRepository.CreatePollAsync(pollEntity);
        var createdPoll = await GetPollByIdAsync(pollEntity.Id);
        return createdPoll;
    }

    public async Task<PollModel> UpdatePollAsync(Guid pollId, UpdatePollModel updatePoll)
    {
        var poll = await pollRepository.GetPollByIdAsync(pollId);
        if (poll == null)
            throw new EntityNotFoundException($"Poll {pollId} not found");
        poll.Title = updatePoll.Title;
        poll.Description = updatePoll.Description;
        await pollRepository.UpdatePollAsync(poll);
        poll = await pollRepository.GetPollByIdAsync(pollId);
        var pollMapped = await GetPollByIdAsync(poll.Id);
        return pollMapped;
    }

    public async Task DeletePollAsync(Guid id)
    {
        await pollRepository.DeletePollAsync(id);
    }

    public async Task<PollModel> AddPollResponseAsync(Guid pollId, Guid optionId)
    {
        var httpContext = httpContextAccessor.HttpContext;
        var userId = JwtHelper.GetUserIdFromToken(httpContext);
        var user = await userService.GetUserByIdAsync((Guid)userId);
        var option = await optionRepository.GetOptionByIdAsync(optionId);
        var poll = await pollRepository.GetPollWithResponsesAsync(pollId);

        if (poll == null)
            throw new EntityNotFoundException($"Poll {pollId} not found");
        if (option == null)
            throw new EntityNotFoundException($"Option {optionId} not found");
        if (user == null)
            throw new EntityNotFoundException($"User {userId} not found");
        if (poll.Id != option.PollId)
            throw new EntityConflictException($"Option ID[{optionId}] doesn't belong to Poll[{pollId}]");// Вариант ответа не из этого опроса
        if (poll.Responses?.Any(m => m.UserId == user.Id) == true)
            throw new EntityConflictException($"User already responded to this poll");// Человек уже голосовал в этом опросе

        var pollResponse = new PollResponse
        {
            PollId = pollId,
            OptionId = optionId,
            UserId = (Guid)userId
        };
        var pollResponses = await pollRepository.AddPollResponseAsync(pollResponse);
        var pollModel = mapper.Map<PollModel>(pollResponses);
        return pollModel;
    }

    public async Task<PollModel> GetResponsesByPollIdAsync(Guid pollId)
    {
        var poll = await pollRepository.GetPollWithResponsesAsync(pollId);
        if (poll == null)
            throw new EntityNotFoundException($"Poll {pollId} not found");

        var pollModel = mapper.Map<PollModel>(poll);
        return pollModel;
    }
}
