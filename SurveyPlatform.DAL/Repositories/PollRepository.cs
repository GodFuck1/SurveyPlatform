using Microsoft.EntityFrameworkCore;
using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DAL.Interfaces;
using SurveyPlatform.DAL.Data;

namespace SurveyPlatform.DAL.Repositories
{
    public class PollRepository : IPollRepository
    {
        private readonly ApplicationDbContext _context;

        public PollRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Poll> GetPollByIdAsync(Guid id)
        {
            return await _context.Polls.Include(p => p.Options).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Poll>> GetAllPollsAsync()
        {
            return await _context.Polls.Include(p => p.Options).ToListAsync();
        }

        public async Task CreatePollAsync(Poll poll)
        {
            await _context.Polls.AddAsync(poll);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePollAsync(Poll poll)
        {
            var existingPoll = await GetPollByIdAsync(poll.Id);
            if (existingPoll != null)
            {
                existingPoll.Title = poll.Title;
                existingPoll.Description = poll.Description;
                existingPoll.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeletePollAsync(Guid id)
        {
            var poll = await GetPollByIdAsync(id);
            if (poll != null)
            {
                _context.Polls.Remove(poll);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddPollResponseAsync(PollResponse response)
        {
            _context.PollResponses.Add(response);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PollResponse>> GetResponsesByPollIdAsync(Guid pollId)
        {
            return await _context.PollResponses.Where(r => r.PollId == pollId).ToListAsync();
        }
    }
}
