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
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async void CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async void DeleteUser(int id)
        {
            var userToDelete = await GetUserById(id);
            _context.Remove(userToDelete);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.Include(p => p.Polls).Include(p => p.Responses).ToList();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

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
