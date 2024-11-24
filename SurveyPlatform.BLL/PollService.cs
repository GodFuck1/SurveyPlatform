using AutoMapper;
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

        public PollService(IPollRepository pollRepository, IMapper mapper)
        {
            _pollRepository = pollRepository;
            _mapper = mapper;
        }

        public async Task<Poll> GetPollById(Guid id)
        {
            return await _pollRepository.GetPollById(id);
        }

        public async Task<IEnumerable<Poll>> GetAllPolls()
        {
            return await _pollRepository.GetAllPolls();
        }

        public async Task CreatePoll(PollModel poll)
        {
            var pollEntity = _mapper.Map<Poll>(poll);
            await _pollRepository.CreatePoll(pollEntity);
        }

        public async Task UpdatePoll(Poll poll)
        {
            await _pollRepository.UpdatePoll(poll);
        }

        public async Task DeletePoll(Guid id)
        {
            await _pollRepository.DeletePoll(id);
        }

        public async Task AddPollResponse(PollResponse response)
        {
            await _pollRepository.AddPollResponse(response);
        }

        public async Task<IEnumerable<PollResponse>> GetResponsesByPollId(Guid pollId)
        {
            return await _pollRepository.GetResponsesByPollId(pollId);
        }
    }
}
