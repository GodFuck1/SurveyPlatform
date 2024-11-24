using SurveyPlatform.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyPlatform.DAL.Interfaces
{
    public interface IPollRepository
    {
        Task<Poll> GetPollById(Guid id);
        Task<IEnumerable<Poll>> GetAllPolls();
        Task CreatePoll(Poll poll);
        Task UpdatePoll(Poll poll);
        Task DeletePoll(Guid id);
        Task AddPollResponse(PollResponse response);
        Task<IEnumerable<PollResponse>> GetResponsesByPollId(Guid pollId);
    }
}
