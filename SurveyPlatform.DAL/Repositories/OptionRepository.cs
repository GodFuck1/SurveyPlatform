using Microsoft.EntityFrameworkCore;
using SurveyPlatform.DAL.Data;
using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DAL.Interfaces;

namespace SurveyPlatform.DAL.Repositories;
public class OptionRepository : IOptionRepository
{
    private readonly ApplicationDbContext _context;
    public OptionRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<PollOption> GetOptionByIdAsync(Guid id)
    {
        return await _context.PollOptions.FirstOrDefaultAsync(p => p.Id == id);
    }
}
