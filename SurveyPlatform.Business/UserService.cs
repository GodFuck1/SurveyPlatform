using AutoMapper;
using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DAL.Interfaces;
using SurveyPlatform.DTOs.Requests;
using SurveyPlatform.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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

        public IEnumerable<UserResponse> GetAllUsers()
        {
            var users = _userRepository.GetAllUsers();
            var usersResponses = _mapper.Map<IEnumerable<UserResponse>>(users);
            return usersResponses;

        }

        public async Task<UserResponse> RegisterUserAsync(RegisterUserRequest userRequest)
        {
            userRequest.Password = HashPassword(userRequest.Password);
            var user = _mapper.Map<User>(userRequest);
            var createdUser = await _userRepository.CreateUser(user);
            var userResponse = _mapper.Map<UserResponse>(createdUser);
            return userResponse;
        }

        public async Task<LoginResponse> LoginUserAsync(LoginUserRequest loginRequest)
        {
            var user = _userRepository.GetAllUsers().FirstOrDefault(u => u.Email == loginRequest.Email);
            if (user == null || !VerifyPassword(loginRequest.Password, user.Password))
            {
                return new LoginResponse { Success = false, Message = "Invalid email or password" };
            }

            var token = _tokenService.GenerateToken(user); // Пример токена

            return new LoginResponse { Success = true, Token = token };
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
