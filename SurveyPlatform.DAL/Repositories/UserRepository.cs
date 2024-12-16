using Microsoft.EntityFrameworkCore;
using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DAL.Interfaces;
using SurveyPlatform.DAL.Data;

namespace SurveyPlatform.DAL.Repositories;
public class UserRepository(
        ApplicationDbContext context
    ) : IUserRepository
{
    public async Task<User> CreateUserAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return await GetUserByIdAsync(user.Id);
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var user = await context.Users.SingleOrDefaultAsync(x => x.Id == id);
        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }
    public async Task ChangeActivateUserAsync(Guid id)
    {
        var user = await GetUserByIdAsync(id);
        user.IsDeactivated = !user.IsDeactivated;
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await context.Users.Include(p => p.Polls).Include(p => p.Responses).ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(Guid id) =>
        await context.Users.
            FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User?> GetUserByEmailAsync(string Email) => 
        await context.Users.
            FirstOrDefaultAsync(u => u.Email == Email);
    

    public async Task<User?> GetUserPollsByIdAsync(Guid id) => 
        await context.Users
            .Include(p => p.Polls)
                .ThenInclude(po => po.Options)
            .FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User?> GetUserResponsesByIdAsync(Guid id) => 
        await context.Users
            .Include(p => p.Responses)
            .FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User> UpdateUserAsync(User user)
    {
        var existingUser = await GetUserByIdAsync(user.Id);
        if (existingUser != null)
        {
            existingUser.Name = user.Name;
            await context.SaveChangesAsync();
            return await GetUserByIdAsync(user.Id);
        }
        else return null;
    }
}
