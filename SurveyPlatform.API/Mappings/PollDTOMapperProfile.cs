using AutoMapper;
using SurveyPlatform.API.DTOs.Requests;
using SurveyPlatform.API.DTOs.Responses;
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
            CreateMap<PollModel, PollDataResponse>(); //полное тело опроса
            CreateMap<PollModel, PollDataShortResponse>(); //без доп полей(автор, ответы итп)
            CreateMap<PollOptionModel, OptionResponse>(); //варианты для голоса
            CreateMap<PollResponseModel, PollSubmittedResponse>(); //голоса людей
            CreateMap<PollResponseModel, PollShortResponseModel>();//голоса людей без поля userId
            CreateMap<UpdatePollRequest, UpdatePollModel>(); //обновление опроса
        }
    }
}
