using SurveyPlatform.DAL.Entities;

namespace SurveyPlatform.DAL.Interfaces
{
    public interface IOptionRepository
    {
        Task<PollOption> GetOptionByIdAsync(Guid id);
    }
}