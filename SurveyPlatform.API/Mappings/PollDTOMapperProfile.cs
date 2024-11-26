using AutoMapper;
using SurveyPlatform.API.DTOs.Requests;
using SurveyPlatform.BLL.Models;
using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DTOs.Responses;

namespace SurveyPlatform.API.Mappings
{
    public class PollDTOMapperProfile:Profile
    {
        public PollDTOMapperProfile()
        {
            CreateMap<CreatePollRequest, PollModel>(); 
            CreateMap<PollModel, PollDataResponse>();
            CreateMap<PollOptionModel, OptionResponse>(); //варианты для голоса
            CreateMap<PollResponseModel, PollSubmittedResponse>(); //голоса людей
            CreateMap<UpdatePollRequest, UpdatePollModel>(); //обновление опроса
        }
    }
}
