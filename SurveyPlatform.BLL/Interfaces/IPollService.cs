using SurveyPlatform.BLL.Models;

namespace SurveyPlatform.BLL.Interfaces;
public interface IPollService
{
    Task<PollModel> AddPollResponseAsync(Guid pollId, Guid optionId);
    Task<PollModel> CreatePollAsync(PollModel poll);
    Task DeletePollAsync(Guid id);
    Task<IEnumerable<PollModel>> GetAllPollsAsync();
    Task<PollModel> GetPollByIdAsync(Guid id);
    Task<PollModel> GetResponsesByPollIdAsync(Guid pollId);
    Task<PollModel> UpdatePollAsync(Guid pollId, UpdatePollModel updatePoll);
}