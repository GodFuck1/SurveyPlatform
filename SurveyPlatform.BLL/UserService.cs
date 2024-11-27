using AutoMapper;
using SurveyPlatform.BLL.Exceptions;
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
                
            userModel.Password = HashPassword(userModel.Password);
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
                throw new EntityNotFoundException($"User {user.Id} not found");
            if (!VerifyPassword(userModel.Password, user.Password))
            {
                return string.Empty;
            }

            var token = _tokenService.GenerateToken(user);

            return token;
        }
        public async Task<UserModel> UpdateUserAsync(Guid id,UpdateUserModel updateUserModel)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
                throw new EntityNotFoundException($"User {id} not found");
            if (!VerifyPassword(updateUserModel.Password, user.Password))
                throw new EntityConflictException("Wrong password");
            user.Name = updateUserModel.Name;
            var updatedUser = await _userRepository.UpdateUser(user);
            var mappedUser = _mapper.Map<UserModel>(updatedUser);
            return mappedUser;
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
                throw new EntityNotFoundException($"User {id} not found");
            await _userRepository.DeleteUser(id);
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        private bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            var hashedInputPassword = HashPassword(inputPassword);
            return hashedInputPassword == hashedPassword;
        }
    }
}
