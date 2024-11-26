using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyPlatform.BLL.Helpers;
using SurveyPlatform.BLL.Models;
using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyPlatform.BLL
{
    public class PollService
    {
        private readonly IPollRepository _pollRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PollService(IPollRepository pollRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _pollRepository = pollRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PollModel> GetPollByIdAsync(Guid id)
        {
            var poll = await _pollRepository.GetPollByIdAsync(id);
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
            await _pollRepository.CreatePollAsync(pollEntity);
            var createdPoll = await GetPollByIdAsync(pollEntity.Id);
            return createdPoll;
        }

        public async Task<bool> UpdatePollAsync(UpdatePollModel updatePoll)
        {
            var poll = await _pollRepository.GetPollByIdAsync(updatePoll.PollId);
            if (poll == null) return false;
            poll.Title = updatePoll.Title;
            poll.Description = updatePoll.Description;
            await _pollRepository.UpdatePollAsync(poll);
            return true;
        }

        public async Task DeletePollAsync(Guid id)
        {
            await _pollRepository.DeletePollAsync(id);
        }

        public async Task<PollModel> AddPollResponseAsync(Guid pollId,Guid optionId)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var userId = JwtHelper.GetUserIdFromToken(httpContext);
            if (userId == null)
            {
                return null;
            }
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
            var pollModel = _mapper.Map<PollModel>(poll);
            return pollModel;
        }
    }
}
