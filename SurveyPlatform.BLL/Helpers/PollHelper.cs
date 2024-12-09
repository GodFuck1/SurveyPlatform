using SurveyPlatform.BLL.Exceptions;
using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DAL.Interfaces;

namespace SurveyPlatform.BLL.Helpers;
public static class PollHelper
{
    /// <summary>
    /// Асинхронный поиск опроса по ID
    /// </summary>
    /// <param name="pollRepository">Репозиторий опросов</param>
    /// <param name="id">pollId</param>
    /// <returns>Poll</returns>
    /// <exception cref="EntityNotFoundException">$"Poll {id} not found"</exception>
    public static async Task<Poll> FindPollByIdAsync(IPollRepository pollRepository, Guid id)
    {
        var poll = await pollRepository.GetPollByIdAsync(id);
        if (poll == null)
            throw new EntityNotFoundException($"Poll {id} not found");
        return poll;
    }
}
