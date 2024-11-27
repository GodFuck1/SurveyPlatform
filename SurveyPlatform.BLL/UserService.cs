using AutoMapper;
using SurveyPlatform.BLL.Exceptions;
using SurveyPlatform.BLL.Helpers;
using SurveyPlatform.BLL.Models;
using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DAL.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SurveyPlatform.BLL
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly TokenService _tokenService;

        public UserService(IUserRepository userRepository, IMapper mapper, TokenService tokenService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<UserModel> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
                throw new EntityNotFoundException($"User {id} not found");
            var userModel = _mapper.Map<UserModel>(user);
            return userModel;
        }

        public async Task<UserResponsesModel> GetUserResponsesByIdAsync(Guid id)
        {
            var user = await _userRepository.GetUserResponsesById(id);
            if (user == null)
                throw new EntityNotFoundException($"User {id} not found");
            var userResponses = _mapper.Map<UserResponsesModel>(user);
            return userResponses;
        }
        public async Task<UserPollsModel> GetUserPollsByIdAsync(Guid id)
        {
            var user = await _userRepository.GetUserPollsById(id);
            if (user == null)
                throw new EntityNotFoundException($"User {id} not found");
            var userPolls = _mapper.Map<UserPollsModel>(user);
            return userPolls;
        }

        public async Task<IEnumerable<UserModel>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            var usersResponses = _mapper.Map<IList<UserModel>>(users);
            return usersResponses;
        }

        public async Task<UserModel> RegisterUserAsync(UserRegisterModel userModel)
        {
            var users = await _userRepository.GetAllUsers();
            var existUser = users.FirstOrDefault(u => u.Email == userModel.Email);
            if (existUser != null)
            {
                throw new EntityConflictException("Email have already been used");
            }
                
            userModel.Password = UserHelper.HashPassword(userModel.Password);
            var user = _mapper.Map<User>(userModel);
            user.Created = DateTime.UtcNow;
            var createdUser = await _userRepository.CreateUser(user);
            var userResponse = _mapper.Map<UserModel>(createdUser);
            return userResponse;
        }

        public async Task<string> LoginUserAsync(UserLoginModel userModel)
        {
            var users = await _userRepository.GetAllUsers();
            var user = users.FirstOrDefault(u => u.Email == userModel.Email);
            if (user == null)
                throw new EntityNotFoundException($"User {userModel.Email} not found");
            if (!UserHelper.VerifyPassword(userModel.Password, user.Password))
            {
                return string.Empty;
            }
            var token = _tokenService.GenerateToken(user);
            return token;
        }
        /// <summary>
        /// Обновить пользователя
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <param name="updateUserModel">Тело обновления</param>
        /// <returns>UserModel</returns>
        /// <exception cref="EntityNotFoundException">Пользователь не найден</exception>
        /// <exception cref="AccessViolationException">Пароль неправильный</exception>
        public async Task<UserModel> UpdateUserAsync(Guid id,UpdateUserModel updateUserModel)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
                throw new EntityNotFoundException($"User {id} not found");
            if (!UserHelper.VerifyPassword(updateUserModel.Password, user.Password))
                throw new AccessViolationException("Wrong password");
            user.Name = updateUserModel.Name;
            var updatedUser = await _userRepository.UpdateUser(user);
            var mappedUser = _mapper.Map<UserModel>(updatedUser);
            return mappedUser;
        }
        /// <summary>
        /// Активировать/деактивировать аккаунт пользователя
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException">Пользователь не найден</exception>
        public async Task ChangeUserActivated(Guid id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
                throw new EntityNotFoundException($"User {id} not found");
            await _userRepository.ChangeActivateUser(id);
        }
        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException">Пользователь не найден</exception>
        public async Task DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
                throw new EntityNotFoundException($"User {id} not found");
            await _userRepository.DeleteUser(id);
        }
    }
}
