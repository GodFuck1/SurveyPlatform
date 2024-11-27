using SurveyPlatform.DAL.Entities;
using AutoMapper;
using SurveyPlatform.BLL.Models;
using SurveyPlatform.DTOs.Responses;
using SurveyPlatform.API.DTOs.Requests;
using SurveyPlatform.API.DTOs.Responses;

namespace SurveyPlatform.BLL.Mappings
{
    public class UserDTOMapperProfile : Profile
    {
        public UserDTOMapperProfile()
        {
            CreateMap<UserModel, UserResponse>();
            CreateMap<UserModel, UpdateUserModel>();
            CreateMap<UpdateUserRequest, UpdateUserModel>();
            CreateMap<UserPollsModel, UserPollsResponse>();
            CreateMap<UserResponsesModel, UserResponsesResponse>();
            CreateMap<RegisterUserRequest, UserRegisterModel>();
            CreateMap<LoginUserRequest, UserLoginModel>();
        }
    }
}
