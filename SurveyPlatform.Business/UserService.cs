using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DAL.Interfaces;
using SurveyPlatform.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyPlatform.Business
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserById(id);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public async Task CreateUserAsync(RegisterUserRequest user)
        {

            //_userRepository.CreateUser(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            _userRepository.UpdateUser(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            _userRepository.DeleteUser(id);
        }
    }
}
