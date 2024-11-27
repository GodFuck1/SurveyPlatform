using SurveyPlatform.DAL.Entities;
using AutoMapper;
using SurveyPlatform.BLL.Models;

namespace SurveyPlatform.BLL.Mappings
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<UserRegisterModel, User>();
            CreateMap<User, UserResponsesModel>();
            CreateMap<User, UserPollsModel>();
            CreateMap<User, UserModel>();
            CreateMap<UserModel, User>();
        }
    }
}
