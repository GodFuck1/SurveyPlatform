﻿using Microsoft.EntityFrameworkCore;
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
        return await GetUserById(user.Id);
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var user = await context.Users.SingleOrDefaultAsync(x => x.Id == id);
        context.Users.Remove(user);
        await context.SaveChangesAsync();
        /*
        using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var userToDelete = await context.Users
                .Include(u => u.Polls)
                .ThenInclude(p => p.Responses)
                .Include(u => u.Responses)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (userToDelete != null)
            {
                foreach (var poll in userToDelete.Polls)
                {
                    //все ответы, связанные с опросами пользователя
                    var responsesToDelete = await context.PollResponses 
                        .Where(r => r.PollId == poll.Id)
                        .ToListAsync();
                    context.PollResponses.RemoveRange(responsesToDelete);
                }
                context.Polls.RemoveRange(userToDelete.Polls);//все опросы пользователя
                context.PollResponses.RemoveRange(userToDelete.Responses);//все ответы пользователя
                context.Users.Remove(userToDelete);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Error deleting user with ID {id}: {ex.Message}");
            throw;
        }*/
    }
    public async Task ChangeActivateUserAsync(Guid id)
    {
        var user = await GetUserById(id);
        user.IsDeactivated = !user.IsDeactivated;
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await context.Users.Include(p => p.Polls).Include(p => p.Responses).ToListAsync();
    }

    public async Task<User?> GetUserById(Guid id)
    {
        return await context.Users.
            FirstOrDefaultAsync(u => u.Id == id);
    }

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
        var existingUser = await GetUserById(user.Id);
        if (existingUser != null)
        {
            existingUser.Name = user.Name;
            await context.SaveChangesAsync();
            return await GetUserById(user.Id);
        }
        else return null;
    }
}
