﻿using SurveyPlatform.DAL.Entities;
using AutoMapper;
using SurveyPlatform.BLL.Models;
using SurveyPlatform.DTOs.Responses;
using SurveyPlatform.API.DTOs.Requests;

namespace SurveyPlatform.BLL.Mappings
{
    public class UserDTOMapperProfile : Profile
    {
        public UserDTOMapperProfile()
        {
            CreateMap<UserModel, UserResponse>();
            CreateMap<UserModel, UserPollsResponse>();
            CreateMap<UserModel, UserResponsesResponse>();
            CreateMap<RegisterUserRequest, UserRegisterModel>();
            CreateMap<LoginUserRequest, UserLoginModel>();
        }
    }
}
