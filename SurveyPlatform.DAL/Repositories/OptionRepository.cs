using Microsoft.EntityFrameworkCore;
using SurveyPlatform.DAL.Data;
using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DAL.Interfaces;

namespace SurveyPlatform.DAL.Repositories;
public class OptionRepository(
        ApplicationDbContext context
    ) : IOptionRepository
{
    public async Task<PollOption> GetOptionByIdAsync(Guid id)
    {
        return await context.PollOptions.FirstOrDefaultAsync(p => p.Id == id);
    }
}
