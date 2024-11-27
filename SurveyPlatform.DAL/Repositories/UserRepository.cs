using Microsoft.EntityFrameworkCore;
using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DAL.Interfaces;
using SurveyPlatform.DAL.Data;

namespace SurveyPlatform.DAL.Repositories;
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    public UserRepository(ApplicationDbContext context) 
    {
        _context = context;
    }


    public async Task<User> CreateUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return await GetUserById(user.Id);
    }

    public async Task DeleteUser(Guid id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var userToDelete = await _context.Users
                .Include(u => u.Polls)
                .ThenInclude(p => p.Responses)
                .Include(u => u.Responses)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (userToDelete != null)
            {
                foreach (var poll in userToDelete.Polls)
                {
                    var responsesToDelete = await _context.PollResponses //все ответы, связанные с опросами пользователя
                        .Where(r => r.PollId == poll.Id)
                        .ToListAsync();
                    _context.PollResponses.RemoveRange(responsesToDelete);
                }
                _context.Polls.RemoveRange(userToDelete.Polls);//все опросы пользователя
                _context.PollResponses.RemoveRange(userToDelete.Responses);//все ответы пользователя
                _context.Users.Remove(userToDelete);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Error deleting user with ID {id}: {ex.Message}");
            throw;
        }
    }
    public async Task ChangeActivateUser(Guid id)
    {
        var user = await GetUserById(id);
        user.IsDeactivated = !user.IsDeactivated;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _context.Users.Include(p => p.Polls).Include(p => p.Responses).ToListAsync();
    }

    public async Task<User> GetUserById(Guid id)
    {
        return await _context.Users.
            FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> GetUserPollsById(Guid id)
    {
        return await _context.Users.
            Include(p => p.Polls).
            FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> GetUserResponsesById(Guid id)
    {
        return await _context.Users.
            Include(p => p.Responses).
            FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> UpdateUser(User user)
    {
        var existingUser = await GetUserById(user.Id);
        if (existingUser != null)
        {
            existingUser.Name = user.Name;
            await _context.SaveChangesAsync();
            return await GetUserById(user.Id);
        }
        else return null;
    }
}
