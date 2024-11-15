using SurveyPlatform.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyPlatform.Core.Interfaces
{
    public interface IPollRepository
    {
        Task<Poll> GetPollById(int id);
        IEnumerable<Poll> GetAllPolls();
        void CreatePoll(Poll poll);
        void UpdatePoll(Poll poll);
        void DeletePoll(int id);
        void AddPollResponse(PollResponse response);
        IEnumerable<PollResponse> GetResponsesByPollId(int pollId);
    }
}
