using SurveyPlatform.DAL.Entities;
using AutoMapper;
using SurveyPlatform.BLL.Models;
using SurveyPlatform.DTOs.Responses;
using SurveyPlatform.API.DTOs.Requests;

namespace SurveyPlatform.Business.Mappings
{
    public class UserDTOMapperProfile : Profile
    {
        public UserDTOMapperProfile() 
        {
            CreateMap<User, UserResponse>();
            CreateMap<RegisterUserRequest, UserModel>();
            CreateMap<LoginUserRequest, UserModel>();
        }
    }
}
