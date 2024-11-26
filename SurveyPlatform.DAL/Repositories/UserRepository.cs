using Microsoft.EntityFrameworkCore;
using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DAL.Interfaces;
using SurveyPlatform.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyPlatform.DAL.Repositories
{
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

        public async void DeleteUser(Guid id)
        {
            var userToDelete = await GetUserById(id);
            _context.Remove(userToDelete);
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

        public async void UpdateUser(User user)
        {
            var existingUser = await GetUserById(user.Id);
            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Password = user.Password;
                await _context.SaveChangesAsync();
            }
        }
    }
}
