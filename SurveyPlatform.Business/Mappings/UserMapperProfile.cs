using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DTOs.Requests;
using SurveyPlatform.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace SurveyPlatform.Business.Mappings
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile() 
        {
            CreateMap<User, UserResponse>();
            CreateMap<RegisterUserRequest, User>();
        }
    }
}
