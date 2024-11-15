using Microsoft.EntityFrameworkCore;
using SurveyPlatform.Core.Entities;
using SurveyPlatform.Core.Interfaces;
using SurveyPlatform.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyPlatform.Infrastructure.Repositories
{
    public class PollRepository : IPollRepository
    {
        private readonly ApplicationDbContext _context;

        public PollRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Poll> GetPollById(int id)
        {
            return await _context.Polls.Include(p => p.Options).FirstOrDefaultAsync(p => p.Id == id);
        }

        public IEnumerable<Poll> GetAllPolls()
        {
            return _context.Polls.Include(p => p.Options).ToList();
        }

        public void CreatePoll(Poll poll)
        {
            _context.Polls.Add(poll);
            _context.SaveChanges();
        }

        public async void UpdatePoll(Poll poll)
        {
            var existingPoll = await GetPollById(poll.Id);
            if (existingPoll != null)
            {
                existingPoll.Title = poll.Title;
                existingPoll.Description = poll.Description;
                existingPoll.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async void DeletePoll(int id)
        {
            var poll = await GetPollById(id);
            if (poll != null)
            {
                _context.Polls.Remove(poll);
                await _context.SaveChangesAsync();
            }
        }

        public async void AddPollResponse(PollResponse response)
        {
            _context.PollResponses.Add(response);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<PollResponse> GetResponsesByPollId(int pollId)
        {
            return _context.PollResponses.Where(r => r.PollId == pollId).ToList();
        }
    }
}
