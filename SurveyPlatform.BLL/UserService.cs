using AutoMapper;
using SurveyPlatform.BLL.Models;
using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DAL.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SurveyPlatform.Business
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

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserById(id);
        }

        public IEnumerable<UserModel> GetAllUsers()
        {
            var users = _userRepository.GetAllUsers();
            var usersResponses = _mapper.Map<IList<UserModel>>(users);
            return usersResponses;
        }

        public async Task<UserModel> RegisterUserAsync(UserRegisterModel userModel)
        {
            var existUser = _userRepository.GetAllUsers().FirstOrDefault(u => u.Email == userModel.Email);
            if (existUser != null) return null;
            userModel.Password = HashPassword(userModel.Password);
            var user = _mapper.Map<User>(userModel);
            var createdUser = await _userRepository.CreateUser(user);
            var userResponse = _mapper.Map<UserModel>(createdUser);
            return userResponse;
        }

        public async Task<string> LoginUserAsync(UserLoginModel userModel)
        {
            var user = _userRepository.GetAllUsers().FirstOrDefault(u => u.Email == userModel.Email);
            if (user == null || !VerifyPassword(userModel.Password, user.Password))
            {
                return string.Empty;
            }

            var token = _tokenService.GenerateToken(user);

            return token;
        }
        public async Task UpdateUserAsync(User user)
        {
            _userRepository.UpdateUser(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            _userRepository.DeleteUser(id);
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
