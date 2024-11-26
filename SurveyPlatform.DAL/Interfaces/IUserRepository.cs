using SurveyPlatform.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyPlatform.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserById(Guid id);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(Guid id);
    }
}
