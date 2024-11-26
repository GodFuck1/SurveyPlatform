using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using SurveyPlatform.BLL.Exceptions;
using SurveyPlatform.BLL.Helpers;
using SurveyPlatform.BLL.Models;
using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DAL.Interfaces;
namespace SurveyPlatform.BLL;
public class PollService
{
    private readonly IPollRepository _pollRepository;
    private readonly UserService _userService;
    private readonly IOptionRepository _optionRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PollService(IPollRepository pollRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IOptionRepository optionRepository, UserService userService)
    {
        _pollRepository = pollRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _optionRepository = optionRepository;
        _userService = userService;
    }

    public async Task<PollModel> GetPollByIdAsync(Guid id)
    {
        var poll = await _pollRepository.GetPollByIdAsync(id);
        if (poll == null)
            throw new EntityNotFoundException($"Poll {id} not found");
        var pollMapped = _mapper.Map<PollModel>(poll);
        return pollMapped;
    }

    public async Task<IEnumerable<PollModel>> GetAllPollsAsync()
    {
        var polls = await _pollRepository.GetAllPollsAsync();
        var pollsMapped = _mapper.Map<IEnumerable<PollModel>>(polls);
        return pollsMapped;
    }

    public async Task<PollModel> CreatePollAsync(PollModel poll)
    {
        var pollEntity = _mapper.Map<Poll>(poll);
        pollEntity.CreatedAt = DateTime.UtcNow;
        await _pollRepository.CreatePollAsync(pollEntity);
        var createdPoll = await GetPollByIdAsync(pollEntity.Id);
        return createdPoll;
    }

    public async Task<PollModel> UpdatePollAsync(UpdatePollModel updatePoll)
    {
        var poll = await _pollRepository.GetPollByIdAsync(updatePoll.PollId);
        if (poll == null)
            throw new EntityNotFoundException($"Poll {updatePoll.PollId} not found");
        poll.Title = updatePoll.Title;
        poll.Description = updatePoll.Description;
        await _pollRepository.UpdatePollAsync(poll);
        poll = await _pollRepository.GetPollByIdAsync(updatePoll.PollId);
        var pollMapped = await GetPollByIdAsync(poll.Id);
        return pollMapped;
    }

    public async Task DeletePollAsync(Guid id)
    {
        await _pollRepository.DeletePollAsync(id);
    }

    public async Task<PollModel> AddPollResponseAsync(Guid pollId,Guid optionId)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var userId = JwtHelper.GetUserIdFromToken(httpContext);
        var user = await _userService.GetUserByIdAsync((Guid)userId);
        var option = await _optionRepository.GetOptionByIdAsync(optionId);
        var poll = await _pollRepository.GetPollWithResponsesAsync(pollId);

        if (poll == null) 
            throw new EntityNotFoundException($"Poll {pollId} not found");
        if (option == null) 
            throw new EntityNotFoundException($"Option {optionId} not found");
        if (user == null) 
            throw new EntityNotFoundException($"User {userId} not found");
        if (poll.Id != option.PollId) 
            throw new EntityConflictException($"Option ID[{optionId}] doesn't belong to Poll[{pollId}]");// Вариант ответа не из этого опроса
        if (poll.Responses?.Any(m=>m.UserId==user.Id) == true) 
            throw new EntityConflictException($"User already responded to this poll");// Человек уже голосовал в этом опросе
        
        var pollResponse = new PollResponse
        {
            PollId = pollId,
            OptionId = optionId,
            UserId = (Guid)userId
        };
        var pollResponses = await _pollRepository.AddPollResponseAsync(pollResponse);
        var pollModel = _mapper.Map<PollModel>(pollResponses);
        return pollModel;
    }

    public async Task<PollModel> GetResponsesByPollIdAsync(Guid pollId)
    {
        var poll = await _pollRepository.GetPollWithResponsesAsync(pollId);
        if (poll == null)
            throw new EntityNotFoundException($"Poll {pollId} not found");

        var pollModel = _mapper.Map<PollModel>(poll);
        return pollModel;
    }
}
