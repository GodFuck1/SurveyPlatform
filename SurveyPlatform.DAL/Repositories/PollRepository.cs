using Microsoft.EntityFrameworkCore;
using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DAL.Interfaces;
using SurveyPlatform.DAL.Data;

namespace SurveyPlatform.DAL.Repositories;
public class PollRepository(
        ApplicationDbContext context
    ) : IPollRepository
{
    public async Task<Poll> GetPollByIdAsync(Guid id)
    {
        return await context.Polls.Include(p => p.Options).FirstOrDefaultAsync(p => p.Id == id);
    }
    public async Task<Poll> GetPollByOptionIdAsync(Guid id)
    {
        return await context.Polls.Include(p => p.Options).FirstOrDefaultAsync(p => p.Options.Any(o => o.Id == id));
    }
    public async Task<IEnumerable<Poll>> GetAllPollsAsync()
    {
        return await context.Polls.Include(p => p.Options).ToListAsync();
    }

    public async Task<Poll> CreatePollAsync(Poll poll)
    {
        await context.Polls.AddAsync(poll);
        await context.SaveChangesAsync();
        return poll;
    }

    public async Task<Poll> UpdatePollAsync(Poll updatingPoll, Poll existingPoll)
    {
        existingPoll.Title = updatingPoll.Title;
        existingPoll.Description = updatingPoll.Description;
        existingPoll.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return existingPoll;
    }

    public async Task DeletePollAsync(Guid id)
    {
        var poll = await GetPollByIdAsync(id);
        if (poll != null)
        {
            context.Polls.Remove(poll);
            await context.SaveChangesAsync();
        }
    }

    public async Task<Poll> AddPollResponseAsync(PollResponse response)
    {
        context.PollResponses.Add(response);
        await context.SaveChangesAsync();
        var poll = await GetPollWithResponsesAsync(response.PollId);
        return poll;
    }

    public async Task<Poll> GetPollWithResponsesAsync(Guid pollId)
    {
        var poll = await context.Polls
            .Include(o=>o.Options)
            .Include(r=>r.Responses)
            .FirstOrDefaultAsync(p => p.Id == pollId);
        return poll;
    }
}
