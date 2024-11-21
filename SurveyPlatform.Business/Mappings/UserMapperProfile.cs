using SurveyPlatform.DAL.Entities;
using AutoMapper;
using SurveyPlatform.BLL.Models;

namespace SurveyPlatform.Business.Mappings
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile() 
        {
            CreateMap<UserModel, User>();
        }
    }
}
