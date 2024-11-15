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

        public Poll GetPollById(int id)
        {
            return _context.Polls.Include(p => p.Options).FirstOrDefault(p => p.Id == id);
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

        public void UpdatePoll(Poll poll)
        {
            var existingPoll = GetPollById(poll.Id);
            if (existingPoll != null)
            {
                existingPoll.Title = poll.Title;
                existingPoll.Description = poll.Description;
                existingPoll.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();
            }
        }

        public void DeletePoll(int id)
        {
            var poll = GetPollById(id);
            if (poll != null)
            {
                _context.Polls.Remove(poll);
                _context.SaveChanges();
            }
        }

        public void AddPollResponse(PollResponse response)
        {
            _context.PollResponses.Add(response);
            _context.SaveChanges();
        }

        public IEnumerable<PollResponse> GetResponsesByPollId(int pollId)
        {
            return _context.PollResponses.Where(r => r.PollId == pollId).ToList();
        }
    }
}
