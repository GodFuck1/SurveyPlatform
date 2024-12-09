using SurveyPlatform.DAL.Entities;

namespace SurveyPlatform.DAL.Interfaces;
public interface IPollRepository
{
    Task<Poll> GetPollByIdAsync(Guid id);
    Task<Poll> GetPollByOptionIdAsync(Guid optionId); 
    Task<Poll> GetPollWithResponsesAsync(Guid pollId); 
    Task<Poll> AddPollResponseAsync(PollResponse response);
    Task<IEnumerable<Poll>> GetAllPollsAsync();
    Task<Poll> CreatePollAsync(Poll poll);
    Task<Poll> UpdatePollAsync(Poll updatingPoll, Poll existingPoll);
    Task DeletePollAsync(Guid id);
}
