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

        public async Task CreatePollAsync(PollModel poll)
        {
            var pollEntity = _mapper.Map<Poll>(poll);
            await _pollRepository.CreatePollAsync(pollEntity);
        }

        public async Task UpdatePollAsync(Poll poll)
        {
            await _pollRepository.UpdatePollAsync(poll);
        }

        public async Task DeletePollAsync(Guid id)
        {
            await _pollRepository.DeletePollAsync(id);
        }

        public async Task AddPollResponseAsync(PollResponse response)
        {
            await _pollRepository.AddPollResponseAsync(response);
        }

        public async Task<IEnumerable<PollResponse>> GetResponsesByPollIdAsync(Guid pollId)
        {
            return await _pollRepository.GetResponsesByPollIdAsync(pollId);
        }
    }
}
